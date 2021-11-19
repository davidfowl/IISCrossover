using System.Text;

namespace MvcMusicStore.Core
{
    public class HttpContextImpl : IHttpContext
    {
        private ISession _session;
        private IPrincipal _user;

        public HttpContextImpl(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor is null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            _session = new SessionImpl(httpContextAccessor);

            _user = new PrincipalImpl(httpContextAccessor);
        }

        public ISession Session => _session;

        public IPrincipal User => _user;
    }

    public class IdentityImpl : IIdentity
    {
        private string? _name;

        public IdentityImpl(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor is null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            _name = httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }

        public string Name => _name ?? string.Empty;
    }

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

    public class SessionImpl : ISession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionImpl(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public object this[string key]
        {
            // TODO: Support more than just strings
            get => Encoding.UTF8.GetString(_httpContextAccessor.HttpContext.Session.Get(key));

            set
            {
                if (!(value is string))
                {
                    throw new NotSupportedException();
                }

                _httpContextAccessor.HttpContext.Session.Set(key, Encoding.UTF8.GetBytes((string)value));
            }
        }
    }
}