namespace MvcMusicStore
{
    public class PrincipalImpl : IPrincipal
    {
        IIdentity _identity = new IndentityImpl();

        public IIdentity Identity => _identity;
    }
}