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
        private JsonFileContent _locationIqRules;
        private string _key;
        private int _apiRequestsLeft;
        private int _requestsPerSecond;
        private DateTime _lastRequestTime = DateTime.UtcNow;
        private DateTime _currentDay;
        private TimeZoneInfo _localZone;
        MongoDatabaseClient _databaseClient;


        public LocationIqClient(string apiConfigPath)
        {
            JsonFileContent configs = new JsonFileContent(apiConfigPath);
            _client = new RestClient((string) configs.selectedParameter("LocationIqUrl"));
            _key = (string) configs.selectedParameter("LocationIqKey");
            _apiRequestsLeft = int.Parse((string) configs.selectedParameter("RequestsPerDay"));
            _requestsPerSecond = int.Parse((string) configs.selectedParameter("RequestsPerSecond"));
            _currentDay = DateTime.UtcNow;
            _localZone = TimeZoneInfo.Local;
            string databaseUrl = (string)configs.selectedParameter("databaseUrl");
            _databaseClient = new MongoDatabaseClient(databaseUrl, "areas");
        }

        public ApiResponse apiRequest(string place)
        {
            if(_databaseClient.Contains(place))
            {
                return _databaseClient.Get(place);
            }
            quantifyRequests();
            var request = new RestRequest(Method.GET);
            string location = place.Trim();
            request.AddParameter("format", "json");
            request.AddParameter("q", location);
            request.AddParameter("key", _key);
            var response = _client.Execute(request);
            var content = response.Content;
            if (locationNotExhist(content))
                throw new ArgumentException($"Unknown location {place}.");
            // Extracts coordinates.
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
                {"area", string.Join(":", areaSplit[areaSplit.Length-3].Trim(),
                                          areaSplit[areaSplit.Length-4].Trim(), 
                                          areaSplit[areaSplit.Length-1].Trim())  }
            };
            _lastRequestTime = DateTime.UtcNow;
            return new ApiResponse(result);
        }

        private bool locationNotExhist(string response)
        {
            string notExhist = "Unable to geocode";
            if (response.Contains(notExhist))
                return true;
            return false;
        }

        private void quantifyRequests()
        {
            var now = DateTime.UtcNow;
            if(_currentDay.Day != now.Day)
            {
                _currentDay = DateTime.UtcNow;
                _apiRequestsLeft = int.Parse((string)_locationIqRules.selectedParameter("RequestsPerDay"));
            }
            if(_lastRequestTime.Ticks - now.Ticks < 1000/ _requestsPerSecond)
            {
                int delay = (1000 /_requestsPerSecond) - (int)(_lastRequestTime.Ticks - now.Ticks);
                System.Threading.Thread.Sleep(delay);
            }
            if(_apiRequestsLeft < 0)
            {
                throw new Exception("LocationIq: No more api requests.");
            }
            _apiRequestsLeft -= 1;
        }
    }
}
