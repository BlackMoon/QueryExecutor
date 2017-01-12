using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using queryExecutor.Controllers;
using queryExecutor.DbManager.Oracle;

namespace queryExecutor
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_EndRequest()
        {
            // 401: Unathorized (ASP.NET uses 401's internally to redirect users to the login page)
            if (Response.StatusCode == 401)
            {
                Response.Clear();

                RouteData rd = new RouteData();
                rd.Values["controller"] = "Error";
                rd.Values["action"] = "Unauthorized";

                IController c = new ErrorController();
                c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
            }
        }

        protected void Application_Error()
        {
            HttpException ex = Server.GetLastError() as HttpException;

            int statusCode = ex?.GetHttpCode() ?? 500;

            Server.ClearError();
            Server.TransferRequest($"/Error/{statusCode}");
        }

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

            RouteTable.Routes.MapRoute(
                    name: "Error",
                    url: "Error/{action}",
                    defaults: new { controller = "Error" }
                );

            // default mvc route
            RouteTable.Routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Index" }
                );
        }
    }
}
