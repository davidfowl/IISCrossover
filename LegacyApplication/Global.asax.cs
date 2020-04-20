using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;


namespace LegacyApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void Application_PostResolveRequestCache(object sender, EventArgs e)
        {
            var controllerName = (string)Context.Request.RequestContext.RouteData.Values["controller"];

            if (!string.IsNullOrEmpty(controllerName))
            {
                var factory = ControllerBuilder.Current.GetControllerFactory();
                IController controller = null;
                try
                {
                    controller = factory.CreateController(Context.Request.RequestContext, controllerName);
                }
                catch
                {
                    // TODO: Use descriptors instead but it's all internal :(
                }

                // The controller doesn't exist, then remap the handler
                if (controller == null)
                {
                    Context.RemapHandler(null);
                }
                else
                {
                    // REVIEW: Figure out if there's an action matching but all of that stuff is hard to get at
                    // var actionName = (string)Context.Request.RequestContext.RouteData.Values["action"];

                    // TODO: Reuse the controller instance with a custom factory (if it hasn't been overridden)

                    // Release the controller
                    factory.ReleaseController(controller);
                }
            }
        }
    }
}
