using System.Web;

namespace MvcMusicStore
{
    public class HttpContextImpl : IHttpContext
    {
        private ISession _session = new SessionImpl();
        private IPrincipal _user = new PrincipalImpl();

        public ISession Session => _session;

        public IPrincipal User => _user;
    }

    public class IdentityImpl : IIdentity
    {
        public string Name
        {
            get
            {
                var name = HttpContext.Current.User.Identity.Name;

                if (name == null)
                {
                    return string.Empty;
                }

                return name;
            }
        }
    }

    public class PrincipalImpl : IPrincipal
    {
        IIdentity _identity = new IdentityImpl();

        public IIdentity Identity => _identity;
    }

    public class SessionImpl : ISession
    {
        public object this[string key]
        {
            get => HttpContext.Current.Session[key];
            set => HttpContext.Current.Session[key] = value;
        }
    }
}