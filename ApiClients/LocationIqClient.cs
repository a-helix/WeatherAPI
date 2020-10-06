using Credentials;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace ApiClients
{
    public class LocationIqClient : IApiClient
    {
        private RestClient _client;
        private string _key;
        private string _url;
        private string _configPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "Configs", "ApiClientKeys.json");

        public LocationIqClient()
        {
            JsonFileContent config = new JsonFileContent(_configPath);
            _client = new RestClient((string)config.selectedParameter("LocationIqUrl"));
            _key = (string) config.selectedParameter("LocationIqKey");
            _url = "/v1/search.php?";
        }

        public ApiResponse apiRequest(string place)
        {
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
            var data = JObject.Parse(coordinates);
            var lat = (string) data["lat"];
            var lon = (string) data["lon"];
            var geolocation = string.Join(";", lat, lon);
            var result = new Dictionary<string, string>()
            {
                {"latitude",  lat },
                {"longitude", lon },
                {"geolocation", geolocation }
            };
            //TODO: Add new place to DB.
            return new ApiResponse(result);
        }

        private bool locationNotExhist(string response)
        {
            string notExhist = "Unable to geocode";
            if (response.Contains(notExhist))
                return true;
            return false;
        }
    }
}
