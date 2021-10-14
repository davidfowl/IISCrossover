using Microsoft.AspNetCore.Http;
using System;

namespace MvcMusicStore.Core
{
    public class PrincipalImpl : IPrincipal
    {
        private readonly IIdentity _identity;

        public PrincipalImpl(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor is null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            _identity = new IdentityImpl(httpContextAccessor);

        }

        public IIdentity Identity => _identity;
    }
}