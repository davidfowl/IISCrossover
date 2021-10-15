using System;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace MvcMusicStore
{
    public class SessionImpl : ISession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISerializer _serializer;

        public SessionImpl(IHttpContextAccessor httpContextAccessor, ISerializer serializer)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new System.ArgumentNullException(nameof(httpContextAccessor));
            this._serializer = serializer ?? throw new System.ArgumentNullException(nameof(serializer));
        }

        public object this[string key]
        {
            // TODO: Support more than just strings
            get => Encoding.UTF8.GetString(_httpContextAccessor.HttpContext.Session.Get(key));

            set
            {
                if (!(value is string))
                {
                    throw new NotSupportedException();
                }

                _httpContextAccessor.HttpContext.Session.Set(key, Encoding.UTF8.GetBytes((string)value));
            }

        }
    }
}