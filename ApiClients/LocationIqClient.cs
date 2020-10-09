using Credentials;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System;

namespace ApiClients
{
    public class LocationIqClient : IApiClient
    {
        private RestClient _client;
        private JsonFileContent _locationIqRules;
        private string _key;
        private string _url;

        private int _apiRequestsLeft;
        private int _requestsPerSecond;
        private DateTime _lastRequestTime;
        private DateTime _currentDay;
        private TimeZoneInfo _localZone;

        public LocationIqClient(string keyConfigPath, string apiRulesPath)
        {
            JsonFileContent keyConfigs = new JsonFileContent(keyConfigPath);
            _client = new RestClient((string)keyConfigs.selectedParameter("LocationIqUrl"));
            _key = (string)keyConfigs.selectedParameter("LocationIqKey");
            _url = "/v1/search.php?";

            _locationIqRules = new JsonFileContent(apiRulesPath);
            _apiRequestsLeft = (int) _locationIqRules.selectedParameter("RequestsPerDay");
            _requestsPerSecond = (int) _locationIqRules.selectedParameter("RequestsPerSecond");
            _currentDay = DateTime.UtcNow;
            _localZone = TimeZoneInfo.Local;
        }

        public ApiResponse apiRequest(string place)
        {
            databaseContains();
            // TODO: Check input in DB first
            var request = new RestRequest(_url, Method.GET);
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
                {"area", string.Join(":", areaSplit[areaSplit.Length-3].Trim(), areaSplit[areaSplit.Length-1].Trim())  }
            };

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
            if(_currentDay != DateTime.UtcNow.Day)
            {
                _currentDay = DateTime.UtcNow.Day;
                _apiRequestsLeft = (int)_locationIqRules.selectedParameter("RequestsPerDay");
            }
            _lastRequestTime = DateTime.UtcNow;
        }

        private bool databaseContains()
        {

        }


    }
}
