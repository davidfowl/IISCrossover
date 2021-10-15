using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcMusicStore.Core
{
    internal class IISCrossoverSession : Microsoft.AspNetCore.Http.ISession
    {
        private Dictionary<string, string> _aspNetFrameworkSession;

        public IISCrossoverSession(Dictionary<string, string> aspNetFrameworkSession)
        {
            _aspNetFrameworkSession = aspNetFrameworkSession;
        }

        public string Id => throw new NotImplementedException();

        public bool IsAvailable => true;

        public IEnumerable<string> Keys => _aspNetFrameworkSession.Keys;

        public void Clear()
        {
            // no-op
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            // no-op
        }

        public void Set(string key, byte[] value)
        {
            // no-op
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (_aspNetFrameworkSession.ContainsKey(key))
            {
                value = Encoding.UTF8.GetBytes(_aspNetFrameworkSession[key]);
                return true;
            }
            value = null;
            return false;
        }
    }
}