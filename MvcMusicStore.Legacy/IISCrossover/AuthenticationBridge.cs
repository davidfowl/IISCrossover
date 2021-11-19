using System;
using System.IO;
using System.Security.Claims;
using System.Web;

namespace MvcMusicStore
{
    public class AuthenticationBridge
    {
        // This magic string can be any value as long as it is unique and shared between the two websites.
        private const string IISCrossoverClaims = "IISCrossover.Claims.77a297cf-0651-4fb6-9c83-73a716e7a2f9";

        public static void ShareUser(HttpContext context)
        {
            if (context == null)
                return;

            var claimsPrincipal = context?.User as ClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                var ms = new MemoryStream();
                var writer = new BinaryWriter(ms);
                claimsPrincipal.WriteTo(writer);

                var serialized = Convert.ToBase64String(ms.ToArray());

                context.Request.ServerVariables[IISCrossoverClaims] = serialized;
            }
        }
    }
}