using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Routing;
using RequestContext = System.Web.Routing.RequestContext;

namespace queryExecutor.Service
{
    /// <summary>
    /// Динамический route для веб-службы
    /// </summary>
    public class DynamicServiceRoute : RouteBase, IRouteHandler
    {
        private readonly string _virtualPath;
        private readonly ServiceRoute _innerServiceRoute;
        private readonly System.Web.Routing.Route _innerRoute;

        public DynamicServiceRoute(string routePath, object defaults, ServiceHostFactoryBase serviceHostFactory, Type serviceType)
        {
            int firstDotIx = serviceType.FullName.IndexOf(".", StringComparison.Ordinal);
            _virtualPath = serviceType.FullName.Substring(firstDotIx + 1).Replace(".", "/");

            _innerServiceRoute = new ServiceRoute(_virtualPath, serviceHostFactory, serviceType);
            _innerRoute = new Route(routePath, new RouteValueDictionary(defaults), this);
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            httpContext.Request.Headers.Add("1", "2");
            return _innerRoute.GetRouteData(httpContext);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            requestContext.HttpContext.RewritePath($"~/{_virtualPath}", true);
            return _innerServiceRoute.RouteHandler.GetHttpHandler(requestContext);
        }
    }
}