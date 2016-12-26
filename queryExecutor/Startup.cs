using System.Web;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using DryIoc;
using DryIoc.WebApi;
using Microsoft.OData.Edm;
using Microsoft.Owin;
using Owin;
using queryExecutor.CQRS.Command;
using queryExecutor.CQRS.Query;
using queryExecutor.DbManager;
using queryExecutor.Domain.DscQColumn;
using queryExecutor.Domain.DscQueryData;
using queryExecutor.Domain.DscQueryParameter;

[assembly: OwinStartup("Startup", typeof(queryExecutor.Startup))]

namespace queryExecutor
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Set up server configuration
            HttpConfiguration config = new HttpConfiguration();

            // prevent [The query specified in the URI is not valid] message
            if (HttpContext.Current.Request.IsLocal)
                config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);

            config.MapODataServiceRoute(routeName: "DscQData", routePrefix: "{datasource}/{path}/{code}/{parameters}/odata", model: GetDataEdmModel());
            config.MapODataServiceRoute(routeName: "DscQuery", routePrefix: "{datasource}/{path}/{code}/odata", model: GetQueryEdmModel());

            #region DI
            IContainer container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient()).WithWebApi(config);
            
            container.RegisterMany(new [] { GetType().Assembly }, (registrator, types, type) =>
            {
                // all dispatchers --> Reuse.InCurrentScope
                IReuse reuse = type.IsAssignableTo(typeof(ICommandDispatcher)) || type.IsAssignableTo(typeof(IQueryDispatcher))
                    ? Reuse.InCurrentScope
                    : Reuse.Transient;

                registrator.RegisterMany(types, type, reuse);
            });

            container.RegisterInstance("Oracle.DataAccess.Client", serviceKey: "ProviderName");
            container.Register(
                reuse: Reuse.InWebRequest,
                made: Made.Of(() => DbManagerFactory.CreateDbManager(Arg.Of<string>("ProviderName"), null), requestIgnored => string.Empty)
                );
           
            container.UseInstance(container);
            #endregion

            appBuilder.UseWebApi(config);
        }

        /// <summary>
        /// Entity Data Model для DscQData
        /// </summary>
        /// <returns></returns>
        private IEdmModel GetDataEdmModel()
        {
            ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<DscQData>("Data");
            
            return modelBuilder.GetEdmModel();
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

            return builder.GetEdmModel();
        }
    }
}
