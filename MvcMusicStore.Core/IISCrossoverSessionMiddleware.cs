using IISCrossover;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Session;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MvcMusicStore.Core
{
    public class IISCrossoverSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public IISCrossoverSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var existingSession = context.Features.Get<ISessionFeature>().Session;
            var sessionString = context.Features.Get<IServerVariablesFeature>()?[IISCrossoverVars.Session];
            if (existingSession != null && sessionString != null)
            {
                var aspNetFrameworkSession = JsonSerializer.Deserialize<Dictionary<string, string>>(sessionString);
                var crossOverSession = new IISCrossoverSession(existingSession, aspNetFrameworkSession);
                context.Features.Set<ISessionFeature>(new SessionFeature() { Session = crossOverSession });
            }

            await _next(context);
        }
    }
}
