namespace MvcMusicStore
{
    public interface ISerializer
    {
        byte[] Serialize<TValue>(object sessionValue);

        TValue Deserialize<TValue>(object SessionValue);
    }
}
