using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using CacheManager.Core;
using Castle.DynamicProxy;
using DryIoc;
using DryIoc.WebApi;
using Microsoft.Owin;
using Owin;
using queryExecutor.CQRS.Query;
using queryExecutor.DbManager;
using queryExecutor.Domain.DscQColumn;
using queryExecutor.Domain.DscQueryData;
using queryExecutor.Domain.DscQueryParameter;
using Microsoft.OData.Edm;
using queryExecutor.Interception;
using queryExecutor.Interception.Attribute;

[assembly: OwinStartup("Startup", typeof(queryExecutor.Startup))]

namespace queryExecutor
{
    /// <summary>
    /// MessageHandler для DscQRoute
    /// <para>Заменяет / в сегменте {path} в odata-url вида {datasource}/{path}/odata. </para>
    /// </summary>
    public class DscQRouteHandler : DelegatingHandler
    {
        private const int WordLength = 3;
        private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789~!";

        private static string _randomWord;

        public static string RandomWord
        {
            get
            {
                if (string.IsNullOrEmpty(_randomWord))
                {
                    Random rnd = new Random();
                    _randomWord = new string(Enumerable.Repeat(Chars, WordLength).Select(s => s[rnd.Next(s.Length)]).ToArray());
                }

                return _randomWord;
            }
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string uri = request.RequestUri.LocalPath;

            Regex rgx = new Regex("^/[\\w.-]+/(.+)/odata");
            Match matches = rgx.Match(uri);

            if (matches.Length > 0)
            {
                foreach (Match m in rgx.Matches(uri))
                {
                    if (m.Groups.Count > 1)
                    {
                        string oldValue = m.Groups[1].Value,
                            newValue = oldValue.Replace("/", RandomWord);

                        uri = uri.Replace(oldValue, newValue);
                    }
                }

                request.RequestUri = new Uri($"{request.RequestUri.Scheme}://{request.RequestUri.Host}:{request.RequestUri.Port}{uri}{request.RequestUri.Query}");
            }

            return base.SendAsync(request, cancellationToken);
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Set up server configuration
            HttpConfiguration config = new HttpConfiguration();

            // prevent [The query specified in the URI is not valid] message
            if (HttpContext.Current.Request.IsLocal)
                config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            // DscQRouteHandler
            config.MessageHandlers.Add(new DscQRouteHandler());

            config.MapODataServiceRoute(
                routeName: "DscQuery", 
                routePrefix: "{datasource}/{path}/odata", 
                model: GetQueryEdmModel());
                
            #region DI
            IContainer container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient()).WithWebApi(config);
            
            container.RegisterMany(new [] { GetType().Assembly }, (registrator, types, type) =>
            {
                Type[] interfaces = type.GetInterfaces();

                bool assignedFromDispatcher = interfaces.Any(i => i.FullName == typeof(IQueryDispatcher).FullName);
                if (assignedFromDispatcher || 
                        interfaces.Any(i => i.FullName == typeof(IInterceptor).FullName || 
                                   i.FullName == typeof(IQuery).FullName || 
                                   (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))))
                {
                    // all dispatchers --> Reuse.InCurrentScope
                    IReuse reuse = assignedFromDispatcher
                        ? Reuse.InCurrentScope
                        : Reuse.Transient;

                    registrator.RegisterMany(types, type, reuse);

                    // interceptors
                    if (type.IsClass)
                    {
                        InterceptedObjectAttribute attr = (InterceptedObjectAttribute)type.GetCustomAttribute(typeof(InterceptedObjectAttribute));
                        if (attr != null)
                        {
                            Type serviceType = attr.ServiceInterfaceType ?? type.GetImplementedInterfaces().FirstOrDefault();
                            registrator.Intercept(serviceType, attr.InterceptorType);
                        }
                    }
                }
            });

            container.RegisterInstance(System.Configuration.ConfigurationManager.AppSettings["ProviderName"], serviceKey: "ProviderName");
            container.Register(
                reuse: Reuse.InWebRequest,
                made: Made.Of(() => DbManagerFactory.CreateDbManager(Arg.Of<string>("ProviderName"), null), requestIgnored => string.Empty)
                );

            // cache manager
            container.Register(reuse: Reuse.Singleton, made: Made.Of(() => CacheFactory.FromConfiguration<object>("webCache")));

            container.UseInstance(container);
            #endregion

            // exception filter
            config.Filters.Add(new GlobalExceptionFilter());

            appBuilder.UseWebApi(config);
        }

        /// <summary>
        /// Entity Data Model для DscQuery
        /// </summary>
        /// <returns></returns>
        private IEdmModel GetQueryEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<DscQColumn>("Columns");
            builder.EntitySet<DscQParameter>("Parameters");
            builder.EntitySet<DscQData>("Results");

            return builder.GetEdmModel();
        }
    }
}
