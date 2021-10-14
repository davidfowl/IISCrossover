using System.Web;

namespace MvcMusicStore
{
    public class IdentityImpl : IIdentity
    {
        public string Name
        {
            get
            {
                var name = HttpContext.Current.User.Identity.Name;

                if (name == null)
                {
                    return string.Empty;
                }

                return name;
            }
        }
    }
}
