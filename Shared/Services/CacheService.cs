using System.Runtime.Caching;
using FoodApi.Shared.Services.Interfaces;

namespace FoodApi.Shared.Services
{
    public class CacheService : ICacheService
    {
        private readonly ObjectCache _memoryCache = MemoryCache.Default;
        public T GetData<T>(string key)
        {

            T item = (T)_memoryCache.Get(key);
            return item;
        }

        public object RemoveData(string key)
        {
            _memoryCache.Remove(key);
            return true;
        }

        public bool SetData<T>(string key, T data, DateTimeOffset expirationTime)
        {
            _memoryCache.Set(key, data, expirationTime);
            return true;
        }


        public List<string> GetAllKeys() => MemoryCache.Default.Select(kv => kv.Key).ToList();
    }
}