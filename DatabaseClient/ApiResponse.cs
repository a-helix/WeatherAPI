using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace DatabaseClient
{
    public class ApiResponse
    {
        private Dictionary<string, string> _cache;

        public ApiResponse(Dictionary<string, string> cache)
        {
            _cache = cache;
        }

        public string value(string parameter)
        {
            return _cache[parameter];
        }

        public int size()
        {
            return _cache.Count;
        }

        public string[] list()
        {
            return _cache.Keys.ToArray();
        }

        public override string ToString()
        {
            string json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
            return json;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            ApiResponse p = (ApiResponse)obj;
            if (p.size() != this.size())
            {
                return false;
            }
            foreach(string i in p.list())
            {
                try
                {
                    if (this.value(i) != p.value(i))
                        return false;
                }
                catch(Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int sum = 0;
            foreach(string i in _cache.Keys.ToArray())
            {
                sum += charSum(i)*2;
            }
            foreach (string i in _cache.Values.ToArray())
            {
                sum += charSum(i)/2;
            }
            return sum;
        }

        private int charSum(string input)
        {
            int sum = 0;
            char[] array = input.ToCharArray();
            foreach(char i in array)
            {
                sum += (int) char.GetNumericValue(i);
            }
            return sum;
        }
    } 
}
