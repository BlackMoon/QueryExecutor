using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using queryExecutor.Domain.DscQueryParameter;
using queryExecutor.Models;

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

            config.MapODataServiceRoute(routeName: "oData", routePrefix: "odata", model: GetEdmModel());

            //config.MapODataServiceRoute(routeName: "Data", routePrefix: "{datasource}/{path}/{name}/{parameters}/odata", model: GetDataEdmModel());
            config.MapODataServiceRoute(routeName: "oData1", routePrefix: "{datasource}/{path}/{name}/odata", model: GetParameterEdmModel());

            appBuilder.UseWebApi(config);
        }

        private IEdmModel GetEdmModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Customer>("Customers");
            //modelBuilder.EntitySet<Order>("Orders");
            //modelBuilder.EntitySet<Customer>("Response");
            //modelBuilder.EntitySet<Employee>("Employees");
            return modelBuilder.GetEdmModel();
        }

        /// <summary>
        /// Edm-модель для DscQData
        /// </summary>
        /// <returns></returns>
        private IEdmModel GetDataEdmModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();
            //modelBuilder.EntitySet<Customer>("Customers");
            //modelBuilder.EntitySet<Order>("Orders");
            //modelBuilder.EntitySet<Customer>("Response");
            //modelBuilder.EntitySet<Employee>("Employees");
            return modelBuilder.GetEdmModel();
        }

        /// <summary>
        /// Edm-модель для DscQParameter
        /// </summary>
        /// <returns></returns>
        private IEdmModel GetParameterEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<DscQParameter>("Parameters");

            return builder.GetEdmModel();
        }
    }
}
