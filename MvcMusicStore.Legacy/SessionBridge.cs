using IISCrossover;
using System.Collections.Generic;
using System.Text.Json;
using System.Web;

namespace MvcMusicStore
{
    public static class SessionBridge
    {
        public static void ShareSession(HttpContext context)
        {
            if (context == null || context.Session == null || context.Session.Count == 0)
            {
                return;
            }

            var dictionary = new Dictionary<string, object>();
            foreach (var key in context.Session.Keys)
            {
                var keyString = key.ToString();
                dictionary.Add(keyString, context.Session[keyString]);
            }

            var sessionString = JsonSerializer.Serialize(dictionary);
            context.Request.ServerVariables[IISCrossoverVars.Session] = sessionString;
        }
    }
}