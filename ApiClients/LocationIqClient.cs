using Credentials;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System;
using DatabaseClient;

namespace ApiClients
{
    public class LocationIqClient : IApiClient
    {
        private RestClient _client;
        private JsonFileContent _configs;
        private string _key;
        private int _apiRequestsLeft;
        private int _requestsPerSecond;
        private DateTime _lastRequestTime = DateTime.UtcNow;
        private DateTime _currentDay;
        

        public LocationIqClient(string apiConfigPath)
        {
            _configs = new JsonFileContent(apiConfigPath);
            _client = new RestClient((string) _configs.Parameter("LocationIqUrl"));
            _key = (string) _configs.Parameter("LocationIqKey");
            _apiRequestsLeft = int.Parse((string) _configs.Parameter("RequestsPerDay"));
            _requestsPerSecond = int.Parse((string) _configs.Parameter("RequestsPerSecond"));
            _currentDay = DateTime.UtcNow;   
        }

        public ApiResponse ApiRequest(string place)
        { 
            QuantifyRequests();
            var content = SendRequest(place);
            var result = ExtractCoordinates(content);
            return new ApiResponse(result);
        }

        private void QuantifyRequests()
        {
            var now = DateTime.UtcNow;
            if (_currentDay.Day != now.Day)
            {
                _currentDay = DateTime.UtcNow;
                _apiRequestsLeft = int.Parse((string)_configs.Parameter("RequestsPerDay"));
            }
            if (now.Ticks -_lastRequestTime.Ticks < 1000 / _requestsPerSecond)
            {
                int delay = (1000 / _requestsPerSecond) - (int)(now.Ticks - _lastRequestTime.Ticks);
                System.Threading.Thread.Sleep(delay);
            }
            if (_apiRequestsLeft <= 0)
            {
                throw new Exception("LocationIq: No more api requests.");
            }
            _apiRequestsLeft -= 1;
        }

        private string SendRequest(string place)
        {
            var request = new RestRequest(Method.GET);
            string location = place.Trim();
            request.AddParameter("format", "json");
            request.AddParameter("q", location);
            request.AddParameter("key", _key);
            var response = _client.Execute(request);
            var content = response.Content;
            if (LocationNotExhist(content))
                throw new ArgumentException($"Unknown location {place}.");
            _lastRequestTime = DateTime.UtcNow;
            return content;
        }

        private Dictionary<string, string> ExtractCoordinates(string content)
        {
            var contentArray = JArray.Parse(content);
            var coordinates = contentArray[0].ToString();
            var areaDescription = contentArray[1].ToString();
            var coordinatesData = JObject.Parse(coordinates);
            var areaDescriptionData = JObject.Parse(areaDescription);
            var lat = (string) coordinatesData["lat"];
            var lon = (string) coordinatesData["lon"];
            var geolocation = string.Join(";", lat, lon);
            var area = (string) areaDescriptionData["display_name"];
            var areaSplit = area.Split(",");
            var result = new Dictionary<string, string>()
            {
                {"latitude",  lat },
                {"longitude", lon },
                {"geolocation", geolocation },
                {
                "area", string.Join(":", areaSplit[areaSplit.Length-3].Trim(),
                                         areaSplit[areaSplit.Length-4].Trim(),
                                         areaSplit[areaSplit.Length-1].Trim())
                }
            };
            return result;
        }

        private bool LocationNotExhist(string response)
        {
            string notExhist = "Unable to geocode";
            if (response.Contains(notExhist))
                return true;
            return false;
        }
    }
}
