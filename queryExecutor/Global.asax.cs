using System;
using System.Configuration;
using System.Reflection;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using queryExecutor.Controllers;
using queryExecutor.DbManager.Oracle;
using queryExecutor.Service;
using queryExecutor.Service.Utils;
using Serilog;

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
                
                Log.Logger.Warning(new UnauthorizedAccessException(Request.Url.AbsoluteUri), string.Empty);
            }
        }

        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();
            HttpException httpEx = ex as HttpException;

            int statusCode = httpEx?.GetHttpCode() ?? 500;
            Log.Logger.Error(ex, string.Empty);

            Server.ClearError();
            Server.TransferRequest($"/Error/{statusCode}");
        }

        protected void Application_Start()
        {
            HostingEnvironment.RegisterVirtualPathProvider(new ResourcePathProvider(Assembly.GetExecutingAssembly()));

            // BASEDIR вставляется в путь serilog:write-to:RollingFile.pathFormat
            Environment.SetEnvironmentVariable("BASEDIR", AppDomain.CurrentDomain.BaseDirectory);       

            Log.Logger = new LoggerConfiguration()
                .ReadFrom
                .AppSettings()
                .CreateLogger();

            // svc route
            RouteTable.Routes.Add(new ServiceRoute("soap/utils.svc", new BehaviorHostFactory(), typeof(Utils)));
                
            // default mvc route
            RouteTable.Routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Index" }
                );
        }
    }
}
