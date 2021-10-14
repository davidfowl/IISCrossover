namespace MvcMusicStore
{
    public interface IPrincipal
    {
        IIdentity Identity { get; }
    }
}
