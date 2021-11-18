using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

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

        // This magic string can be any value as long as it is unique and shared between the two websites.
        private const string IISCrossovrClaims = "IISCrossover.Claims.77a297cf-0651-4fb6-9c83-73a716e7a2f9";

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
            var serializedClaimsPrincipal = Context.GetServerVariable(IISCrossovrClaims);
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