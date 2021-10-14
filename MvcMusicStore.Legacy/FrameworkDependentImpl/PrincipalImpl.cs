namespace MvcMusicStore
{
    public class PrincipalImpl : IPrincipal
    {
        IIdentity _identity = new IdentityImpl();

        public IIdentity Identity => _identity;
    }
}