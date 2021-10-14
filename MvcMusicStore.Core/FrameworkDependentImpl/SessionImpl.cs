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
            get => _httpContextAccessor.HttpContext.Session.Get(key);

            set => _httpContextAccessor.HttpContext.Session.Set(key, _serializer.Serialize<string>(value));
        }
    }
}