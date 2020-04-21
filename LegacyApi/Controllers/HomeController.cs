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
                new Link{ Framework = "ASP.NET Core", Url = HttpContext.Request.Url + "api/weather" }
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