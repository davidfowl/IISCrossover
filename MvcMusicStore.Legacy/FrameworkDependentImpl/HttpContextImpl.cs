namespace MvcMusicStore
{
    public class HttpContextImpl : IHttpContext
    {
        private ISession _session = new SessionImpl();
        private IPrincipal _user = new PrincipalImpl();

        public ISession Session => _session;

        public IPrincipal User => _user;
    }
}