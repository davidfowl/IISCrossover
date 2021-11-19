namespace MvcMusicStore
{
    public interface IHttpContext
    {
        ISession Session { get; }

        IPrincipal User { get; }
    }

    public interface IIdentity
    {
        string Name { get; }
    }

    public interface IPrincipal
    {
        IIdentity Identity { get; }
    }

    public interface ISession
    {
        object this[string key] { get; set; }
    }
}