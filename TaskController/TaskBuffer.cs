using DatabaseClient;
using System.Collections.Generic;

namespace TaskController
{
    public static class TaskBuffer
    {
        private static Dictionary<string, ApiResponse> _cache = new Dictionary<string, ApiResponse>();

        public static void Add(string key, ApiResponse value)
        {
            lock(_cache)
            {
                _cache.Add(key, value);
            }
        }

        public static ApiResponse Get(string key)
        {
            lock (_cache)
            {
                return _cache[key];
            }
        }

        public static void Delete(string key)
        {
            lock(_cache)
            {
                _cache.Remove(key);
            }
        }
    }
}
