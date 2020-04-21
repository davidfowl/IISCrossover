using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace LegacyApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void Application_PostResolveRequestCache(object sender, EventArgs e)
        {
            // Here we're going to decide which routes map to ASP.NET Core and which ones map to ASP.NET 
            // - /api/weather/ to ASP.NET Core
            // - /api/values to ASP.NET

            // At this point we've set the handler to take this request, now if this route maps to one
            // we want to redirect to ASP.NET Core, so set the handler to null
            if (Context.Request.Path.StartsWith("/api/weather", StringComparison.OrdinalIgnoreCase))
            {
                Context.RemapHandler(null);
            }
        }
    }
}
