using IISCrossover.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Web;

namespace MvcMusicStore
{
    public class AuthenticationBridge
    {
        public static void ShareUser(HttpContext context)
        {
            var claimsPrincipal = context?.User as ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                var buffer = new Dictionary<string, string>();
                foreach(var claim in claimsPrincipal.Claims)
                {
                    buffer[claim.Type] = claim.Value;
                }

                context.Request.ServerVariables[CrossoverAuthenticationVars.Claims] = JsonSerializer.Serialize(buffer);
            }
        }
    }
}