using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using IISCrossover;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Session;

namespace MvcMusicStore.Core
{
    public class IISCrossoverSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public IISCrossoverSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var sessionString = context.GetServerVariable(IISCrossoverVars.Session);
            if (sessionString != null)
            {
                var aspNetFrameworkSession = JsonSerializer.Deserialize<Dictionary<string, string>>(sessionString);
                var crossOverSession = new IISCrossoverSession(aspNetFrameworkSession);
                context.Features.Set<ISessionFeature>(new SessionFeature() { Session = crossOverSession });
            }

            return _next(context);
        }
    }
}
