using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Session;
using System.Text.Json;

namespace MvcMusicStore.Core
{
    public class IISCrossoverSessionMiddleware
    {
        // This magic string can be any value as long as it is unique and shared between the two websites.
        private const string IISCrossoverSession = "IISCrossover.Session.77a297cf-0651-4fb6-9c83-73a716e7a2f9";

        private readonly RequestDelegate _next;

        public IISCrossoverSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var sessionString = context.GetServerVariable(IISCrossoverSession);
            if (sessionString != null)
            {
                var aspNetFrameworkSession = JsonSerializer.Deserialize<Dictionary<string, string>>(sessionString);
                if (aspNetFrameworkSession != null)
                {
                    var crossOverSession = new IISCrossoverSession(aspNetFrameworkSession);
                    context.Features.Set<ISessionFeature>(new SessionFeature() { Session = crossOverSession });
                }
            }

            return _next(context);
        }
    }
}