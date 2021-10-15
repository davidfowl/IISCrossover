using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
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
            var bufferText = Context.GetServerVariable(IISCrossoverVars.Claims);
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(bufferText))
            {
                try
                {
                    var buffer = JsonSerializer.Deserialize<Dictionary<string, string>>(bufferText);

                    foreach (var kvp in buffer)
                    {
                        claims.Add(new Claim(kvp.Key, kvp.Value));
                    }
                }
                catch
                {
                    Task.FromResult(AuthenticateResult.Fail("Unable to process authenticated user from server variable."));
                }
            }

            if (claims.Count > 0)
            {
                // generate claimsIdentity on the name of the class
                var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);

                // generate AuthenticationTicket from the Identity and current authentication scheme
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);

                // pass on the ticket to the middleware
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}
