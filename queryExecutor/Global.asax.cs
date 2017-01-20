using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text.RegularExpressions;
using System.Web;
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

        /// <summary>
        /// Заменяет / в сегменте {path} в url вида {datasource}/{path}/odata.
        /// </summary>
        protected void Application_BeginRequest()
        {
            string uri = Request.Path;

            Regex rgx = new Regex("^/[\\w.-]+/(.+)/odata", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match match = rgx.Match(uri);

            if (match.Success)
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

                HttpContext.Current.RewritePath($"{uri}{Request.Url.Query}");
            }
        }

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
            // BASEDIR вставляется в путь serilog:write-to:RollingFile.pathFormat
            Environment.SetEnvironmentVariable("BASEDIR", AppDomain.CurrentDomain.BaseDirectory);       

            Log.Logger = new LoggerConfiguration()
                .ReadFrom
                .AppSettings()
                .CreateLogger();

            OracleEnvironmentConfiguration config = (OracleEnvironmentConfiguration)ConfigurationManager.GetSection("oracleEnvironment");
        
            if (!string.IsNullOrEmpty(config.Nls_Lang))
                Environment.SetEnvironmentVariable("NLS_LANG", config.Nls_Lang);

            if (!string.IsNullOrEmpty(config.Oracle_Home))
                Environment.SetEnvironmentVariable("ORACLE_HOME", config.Oracle_Home);

            if (!string.IsNullOrEmpty(config.Path))
                Environment.SetEnvironmentVariable("PATH", $"{config.Path};{Environment.GetEnvironmentVariable("PATH")}");

            if (!string.IsNullOrEmpty(config.Tns_Admin))
                Environment.SetEnvironmentVariable("TNS_ADMIN", config.Tns_Admin);

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
