using Newtonsoft.Json;
using System.Collections.Generic;

namespace ApiClients
{
    public class ApiResponse
    {
        private Dictionary<string, string> _cache;

        public ApiResponse(Dictionary<string, string> cache)
        {
            this._cache = cache;
        }

        public string value(string parameter)
        {
            return _cache[parameter];
        }

        public override string ToString()
        {
            string json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
            return json;
        }
    } 
}
