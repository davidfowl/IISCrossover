using System;
using System.IO;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IISCrossover;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MvcMusicStore.Core
{
    public static class IISCrossoverAuthenticationDefaults
    {
        /// <summary>
        /// The default value used for AuthenticationSchemeOptions.AuthenticationScheme
        /// </summary>
        public const string AuthenticationScheme = "IISCrossOver";
    }

    public class IISCrossoverAuthenticationSchemeOptions
        : AuthenticationSchemeOptions
    {
    }

    public class IISCrossoverAuthenticationHandler
        : AuthenticationHandler<IISCrossoverAuthenticationSchemeOptions>
    {
        public IISCrossoverAuthenticationHandler(
            IOptionsMonitor<IISCrossoverAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var serializedClaimsPrincipal = Context.GetServerVariable(IISCrossoverVars.Claims);
            ClaimsPrincipal claimsPrincipal = null;

            if (serializedClaimsPrincipal != null)
            {
                try
                {
                    var bytes = Convert.FromBase64String(serializedClaimsPrincipal);

                    claimsPrincipal = new ClaimsPrincipal(new BinaryReader(new MemoryStream(bytes)));
                }
                catch
                {
                    return Task.FromResult(AuthenticateResult.Fail("Unable to process authenticated user from server variable."));
                }
            }

            if (claimsPrincipal != null)
            {
                // generate AuthenticationTicket from the Identity and current authentication scheme
                var ticket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

                // pass on the ticket to the middleware
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
