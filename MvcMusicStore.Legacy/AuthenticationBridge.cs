using IISCrossover;
using System;
using System.IO;
using System.Security.Claims;
using System.Web;

namespace MvcMusicStore
{
    public class AuthenticationBridge
    {
        public static void ShareUser(HttpContext context)
        {
            if (context == null)
                return;

            var claimsPrincipal = context?.User as ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                // TODO: Efficiency
                var ms = new MemoryStream();
                var writer = new BinaryWriter(ms);
                claimsPrincipal.WriteTo(writer);

                var serialized = Convert.ToBase64String(ms.ToArray());

                context.Request.ServerVariables[IISCrossoverVars.Claims] = serialized;
            }
        }
    }
}