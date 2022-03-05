using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningContest.Application.Contracts.Services
{
    public interface IRedisCacheService
    {
        Task<string> GetPlainText(string key);
        Task<T> Get<T>(string key);
        Task<T> Set<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null);
        Task<string> SetPlainText(string key, string value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null);

        Task Remove(string key); 
    }
    public class RedisCacheService: IRedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<string> GetPlainText(string key)
        {
            var value = await _cache.GetStringAsync(key);

            if (value != null)
            {
                return value; 
            }

            return default;
        }

        public async Task<T> Get<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);
            
            if (value != null)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }

        public async Task<T> Set<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration,
                SlidingExpiration = slidingExpiration,
                
            };

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; 
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value, settings), options);

            return value;
        }
        public async Task<string> SetPlainText(string key, string value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration,
                SlidingExpiration = slidingExpiration
            };

            await _cache.SetStringAsync(key, value, options);
            return value;
        }

        public async Task Remove(string key)
        {
            await _cache.RemoveAsync(key); 
        }
    }
}
