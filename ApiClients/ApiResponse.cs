using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

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

        public string json()
        {
            string json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
            return json;
        }
    } 
}
