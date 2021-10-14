using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcMusicStore.Core
{
    internal class IISCrossoverSession : ISession
    {
        private readonly ISession _originalSession;
        private Dictionary<string, string> _aspNetFrameworkSession;

        public IISCrossoverSession(ISession originalSession, Dictionary<string, string> aspNetFrameworkSession)
        {
            _originalSession = originalSession;
            _aspNetFrameworkSession = aspNetFrameworkSession;
        }

        public string Id => _originalSession.Id;

        public bool IsAvailable => _originalSession.IsAvailable;

        public IEnumerable<string> Keys => _aspNetFrameworkSession.Keys.Union(_originalSession.Keys);

        public void Clear()
        {
            _originalSession.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return _originalSession.CommitAsync(cancellationToken);
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return _originalSession.LoadAsync(cancellationToken);
        }

        public void Remove(string key)
        {
            _originalSession.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _originalSession.Set(key, value);
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (_aspNetFrameworkSession.ContainsKey(key))
            {
                value = Encoding.UTF8.GetBytes(_aspNetFrameworkSession[key]);
                return true;
            }

            return _originalSession.TryGetValue(key, out value);
        }
    }
}