namespace MvcMusicStore
{
    public interface IHttpContext
    {
        ISession Session { get; }

        IPrincipal User { get; }
    }
}
