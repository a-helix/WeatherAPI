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
        MongoDatabaseClient _databaseClient;
        

        public LocationIqClient(string apiConfigPath)
        {
            _configs = new JsonFileContent(apiConfigPath);
            _client = new RestClient((string) _configs.Parameter("LocationIqUrl"));
            _key = (string) _configs.Parameter("LocationIqKey");
            _apiRequestsLeft = int.Parse((string) _configs.Parameter("RequestsPerDay"));
            _requestsPerSecond = int.Parse((string) _configs.Parameter("RequestsPerSecond"));
            _currentDay = DateTime.UtcNow;
            string databaseUrl = (string) _configs.Parameter("databaseUrl");
            _databaseClient = new MongoDatabaseClient(databaseUrl, "areas", "areas");
        }

        public ApiResponse ApiRequest(string place)
        {
            ApiResponse dbResponse = _databaseClient.Get(place);
            if (dbResponse != null)
            {
                return dbResponse;
            }
            quantifyRequests();
            var request = new RestRequest(Method.GET);
            string location = place.Trim();
            request.AddParameter("format", "json");
            request.AddParameter("q", location);
            request.AddParameter("key", _key);
            var response = _client.Execute(request);
            var content = response.Content;
            if (LocationNotExhist(content))
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
                {
                "area", string.Join(":", areaSplit[areaSplit.Length-3].Trim(),
                                            areaSplit[areaSplit.Length-4].Trim(), 
                                            areaSplit[areaSplit.Length-1].Trim())  
                }
            };
            _lastRequestTime = DateTime.UtcNow;
            return new ApiResponse(result);
        }

        private bool LocationNotExhist(string response)
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
                _apiRequestsLeft = int.Parse((string) _configs.Parameter("RequestsPerDay"));
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
