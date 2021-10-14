using System.Web;

namespace MvcMusicStore
{
    public class IndentityImpl : IIdentity
    {
        public string Name
        {
            get => HttpContext.Current.User.Identity.Name;
        }
    }
}
