using System.Runtime.Versioning;
using System.Web.Mvc;

namespace LegacyApi.Controllers
{
    public class HomeController : Controller
    {
        // GET api/<controller>
        public ActionResult Index()
        {
            var data = new[] {
                new Link{ Framework = "ASP.NET WebAPI", Url = HttpContext.Request.Url + "api/values" },
                new Link{ Framework = "ASP.NET Core", Url = HttpContext.Request.Url + "api/weather" },
                new Link{ Framework = "ASP.NET Core WebForm Page (redirected)", Url = HttpContext.Request.Url + "WebForm1.aspx" },
                new Link{ Framework = "ASP.NET WebForm Page", Url = HttpContext.Request.Url + "WebForm2.aspx" }
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private class Link
        {
            public string Framework { get; set; }
            public string Url { get; set; }
        }
    }
}