using System.Web;

namespace MvcMusicStore
{
    public class SessionImpl : ISession
    {
        public object this[string key]
        {
            get => HttpContext.Current.Session[key];
            set => HttpContext.Current.Session[key] = value;
        }
    }
}