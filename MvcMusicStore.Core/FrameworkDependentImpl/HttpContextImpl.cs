using Microsoft.AspNetCore.Http;
using System;

namespace MvcMusicStore.Core
{
    public class HttpContextImpl : IHttpContext
    {
        private ISession _session;
        private IPrincipal _user;

        public HttpContextImpl(IHttpContextAccessor httpContextAccessor, ISerializer serializer)
        {
            if (httpContextAccessor is null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            if (serializer is null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            _session = new SessionImpl(httpContextAccessor, serializer);

            _user = new PrincipalImpl(httpContextAccessor);
        }

        public ISession Session => _session;

        public IPrincipal User => _user;
    }
}