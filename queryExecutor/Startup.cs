using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter;
using System.Web.OData.Formatter.Deserialization;
using CacheManager.Core;
using Castle.DynamicProxy;
using DryIoc;
using DryIoc.WebApi;
using Microsoft.OData.Edm;
using Microsoft.Owin;
using Owin;
using queryExecutor.CQRS.Job;
using queryExecutor.CQRS.Query;
using queryExecutor.DbManager;
using queryExecutor.DbManager.Oracle;
using queryExecutor.Domain.DscQColumn;
using queryExecutor.Domain.DscQueryData;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Interception;
using queryExecutor.Interception.Attribute;
using queryExecutor.OData;
using Serilog;

[assembly: OwinStartup("Startup", typeof(queryExecutor.Startup))]

namespace queryExecutor
{
    public class Startup
    {
        public static IContainer Container;
        
        public void Configuration(IAppBuilder appBuilder)
        {
            // Set up server configuration
            HttpConfiguration config = new HttpConfiguration();

            // DscQRouteHandler
            config.MessageHandlers.Add(new DscQRouteHandler());

            config.MapODataServiceRoute(
                routeName: "DscQuery", 
                routePrefix: "{datasource}/{path}/odata", 
                model: GetQueryEdmModel());
                
            #region DI
            Container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient()).WithWebApi(config);
            
            Container.RegisterMany(new [] { GetType().Assembly }, (registrator, types, type) =>
            {
                Type[] interfaces = type.GetInterfaces();

                bool assignedFromDispatcher = interfaces.Any(i => i.IsAssignableTo(typeof(IJobDispatcher)) || i.IsAssignableTo(typeof (IQueryDispatcher)));
                
                if (assignedFromDispatcher || interfaces.Any(i => i.IsAssignableTo(typeof(IInterceptor)) || 
                    i.IsAssignableTo(typeof(IJob)) || 
                    i.IsAssignableTo(typeof(IQuery)) ||
                    (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))))
                {
                    // all dispatchers --> Reuse.InCurrentScope
                    IReuse reuse = assignedFromDispatcher
                        ? Reuse.InResolutionScope
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

            Container.RegisterInstance((IOracleEnvironmentConfiguration)System.Configuration.ConfigurationManager.GetSection("oracleEnvironment"));
            Container.RegisterInstance(System.Configuration.ConfigurationManager.AppSettings["ProviderName"], serviceKey: "ProviderName");
            Container.Register(
                reuse: Reuse.InResolutionScope,
                made: Made.Of(() => DbManagerFactory.CreateDbManager(Arg.Of<string>("ProviderName"), null), requestIgnored => string.Empty)
                );

            // IDbManager initialiazation
            Container.RegisterInitializer<IDbManager>((m, r) => m.Log = Log.Logger.Debug);

            // cache manager
            Container.Register(reuse: Reuse.Singleton, made: Made.Of(() => CacheFactory.FromConfiguration<object>("webCache")));

            Container.UseInstance(Container);
            #endregion

            // Startup Jobs
            IJobDispatcher dispatcher = Container.Resolve<IJobDispatcher>(IfUnresolved.ReturnDefault);
            dispatcher?.Dispatch<IStartupJob>();

            config.Filters.Add(new GlobalExceptionFilter());
            
            var odataFormatters = ODataMediaTypeFormatters.Create(new SkipNullValueODataSerializerProvider(), new DefaultODataDeserializerProvider());
            config.Formatters.InsertRange(0, odataFormatters);
            
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
