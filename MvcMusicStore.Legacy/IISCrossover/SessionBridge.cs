using System.Collections.Generic;
using System.Text.Json;
using System.Web;

namespace MvcMusicStore
{
    public static class SessionBridge
    {
        // This magic string can be any value as long as it is unique and shared between the two websites.
        private const string IISCrossoverSession = "IISCrossover.Session.77a297cf-0651-4fb6-9c83-73a716e7a2f9";

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
            context.Request.ServerVariables[IISCrossoverSession] = sessionString;
        }
    }
}