using System;
using System.Web;
using System.Web.Security;

namespace IISCrossover.Authentication.Net
{
    public class AuthenticationBridge
    {
        public static void ShareUser(HttpContext context)
        {
            if (context.User != null && context.User.Identity != null)
            {
                // assumes your user only has one identity
                context.Request.ServerVariables[CrossoverAuthenticationVars.UserName] = context.User.Identity.Name;

                // TODO - discuss how to make this more safe without bloating the project
                var roles = Roles.GetRolesForUser(context.User.Identity.Name);
                if (roles != null)
                {
                    context.Request.ServerVariables[CrossoverAuthenticationVars.Roles] = string.Join(",", roles);
                }
            }
        }
    }
}
