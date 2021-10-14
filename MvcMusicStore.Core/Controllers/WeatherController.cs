using IISCrossover.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MvcMusicStore.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherController> _logger;

        public WeatherController(ILogger<WeatherController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("debug")]
        public string Debug()
        {
            string userNameText = "NotAuthenticated";
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                userNameText = User.Identity.Name;
            }

            var responseText = $"Welcome {userNameText}{Environment.NewLine}";

            responseText += $"Claims: {Environment.NewLine}";
            foreach(var claim in User.Claims)
            {
                responseText += $"    {claim.Type}:{claim.Value}{Environment.NewLine}";
            }

            responseText += RoleCheck("Administrator");
            responseText += RoleCheck("WeatherReader");

            return responseText;

            string RoleCheck(string roleName)
            {
                return $"UserIsInRole {roleName}: {User.IsInRole(roleName)}{Environment.NewLine}";
            }
        }

        [HttpGet("pid")]
        public int GetPid()
        {
            return Process.GetCurrentProcess().Id;
        }
    }
}
