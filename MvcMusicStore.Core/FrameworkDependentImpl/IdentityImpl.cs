using Microsoft.AspNetCore.Http;
using System;

namespace MvcMusicStore.Core
{
    public class IdentityImpl : IIdentity
    {
        private string _name;

        public IdentityImpl(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor is null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            _name = httpContextAccessor.HttpContext.User.Identity.Name;
            if (_name == null)
            {
                _name = string.Empty;
            }
        }

        public string Name => _name;
    }
}
