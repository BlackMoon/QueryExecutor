using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using queryExecutor.DbManager.Oracle;

namespace queryExecutor
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            OracleEnvironmentConfiguration config = (OracleEnvironmentConfiguration)ConfigurationManager.GetSection("oracleEnvironment");
        
            if (!string.IsNullOrEmpty(config.Nls_Lang))
                Environment.SetEnvironmentVariable("NLS_LANG", config.Nls_Lang);

            if (!string.IsNullOrEmpty(config.Oracle_Home))
                Environment.SetEnvironmentVariable("ORACLE_HOME", config.Oracle_Home);

            if (!string.IsNullOrEmpty(config.Path))
                Environment.SetEnvironmentVariable("PATH", config.Path + ";" + Environment.GetEnvironmentVariable("PATH"));

            if (!string.IsNullOrEmpty(config.Tns_Admin))
                Environment.SetEnvironmentVariable("TNS_ADMIN", config.Tns_Admin);
            
            // default mvc route
            RouteTable.Routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Index" }
                );
        }
    }
}
