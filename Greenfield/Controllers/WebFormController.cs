using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Greenfield.Controllers
{
    [ApiController]
    [Route("WebForm1.aspx")]
    public class WebFormController : ControllerBase
    {
        [HttpGet]
        public string Get([FromServices]IWebHostEnvironment webHostEnvironment)
        {
            // We can pass state between ASP.NET and ASP.NET Core using server variables
            return $"GREENFIELD ASPX, Content Root: {webHostEnvironment.ContentRootPath}, From Server Variables: {HttpContext.Features.Get<IServerVariablesFeature>()?["FromFramework"]}";
        }
    }
}