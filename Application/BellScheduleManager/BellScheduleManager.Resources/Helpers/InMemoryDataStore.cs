using Google.Apis.Util.Store;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace BellScheduleManager.Resources.Helpers
{
    public class InMemoryDataStore : IDataStore
    {
        private const string _keyPrefix = "googledatastore_";
        private readonly IMemoryCache _cache;

        public InMemoryDataStore(IMemoryCache cache)
        {
            _cache = cache;
        }

        private string FormattedKey(string keySuffix) => $"{_keyPrefix}{keySuffix}";

        public Task ClearAsync()
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync<T>(string key)
        {
            _cache.Remove(FormattedKey(key));
            return Task.CompletedTask;
        }

        public Task<T> GetAsync<T>(string key)
        {
            var val = _cache.Get<T>(FormattedKey(key));
            return Task.FromResult(val);
        }

        public Task StoreAsync<T>(string key, T value)
        {
            var val = _cache.Set(FormattedKey(key), value);
            return Task.CompletedTask;
        }
    }
}
