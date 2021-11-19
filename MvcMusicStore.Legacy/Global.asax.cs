using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace MvcMusicStore
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer(new MvcMusicStore.Models.SampleData());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private void Application_PostResolveRequestCache(object sender, EventArgs e)
        {
            // At this point we've set the handler to take this request, now if this route maps to one
            // we want to redirect to ASP.NET Core, so set the handler to null
            if (Context.Request.Path.StartsWith("/ShoppingCart", StringComparison.OrdinalIgnoreCase))
            {
                // We want to share session for requests that make it to ASP.NET Core
                Context.SetSessionStateBehavior(SessionStateBehavior.Required);

                AuthenticationBridge.ShareUser(Context);

                Context.RemapHandler(null);
            }
        }

        private void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (Context.Request.Path.StartsWith("/ShoppingCart", StringComparison.OrdinalIgnoreCase))
            {
                SessionBridge.ShareSession(Context);
            }
        }
    }
}