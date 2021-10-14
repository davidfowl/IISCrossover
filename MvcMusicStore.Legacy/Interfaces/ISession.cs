namespace MvcMusicStore
{
    public interface ISession
    {
        object this[string key] { get; set; }
    }
}