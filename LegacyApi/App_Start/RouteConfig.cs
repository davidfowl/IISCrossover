using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace LegacyApi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Since route matching doesn't support both catch all routes and extensions, we use a route constraint
            // to prevent matches against non-aspx extensions
            routes.Add("Pages", new Route("{*path}", defaults: null, constraints: new RouteValueDictionary(new { path = new AspxFileConstraint() }), new PagesRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        private class PagesRouteHandler : IRouteHandler
        {
            public IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                Debug.Assert(VirtualPathUtility.GetExtension(requestContext.HttpContext.Request.Path).Equals(".aspx", StringComparison.OrdinalIgnoreCase), "Not an aspx file!");

                var virtualPath = VirtualPathUtility.ToAppRelative(requestContext.HttpContext.Request.Path);

                return (Page)BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(Page));
            }
        }

        private class AspxFileConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                return httpContext.Request.Path.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
