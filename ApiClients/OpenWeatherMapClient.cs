using System;
using System.Collections.Generic;
using Credentials;
using DatabaseClient;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ApiClients
{
    public class OpenWeatherMapClient : IApiClient
    {
        private RestClient _client;
        private string _key;

        public OpenWeatherMapClient(string configPath)
        {
            JsonFileContent config = new JsonFileContent(configPath);
            _client = new RestClient((string)config.Value("OpenWeatherMapUrl"));
            _key = (string) config.Value("OpenWeatherMapKey"); 
        }

        public ApiResponse ApiRequest(string geolocation)
        {
            // Throws ArgumentException if coordinates are unrealistic.
            ValidCoordinates(geolocation);
            //Generates an HTTP request.
            var request = new RestRequest(Method.GET);
            string[] coordinates = geolocation.Split(";");
            double latitude = double.Parse(coordinates[0]);
            double longitude = double.Parse(coordinates[1]);
            request.AddParameter("lat", latitude);
            request.AddParameter("lon", longitude);
            request.AddParameter("appid", _key);
            var response = _client.Execute(request);


            var content = response.Content;
            JObject json = JObject.Parse(content);
            Dictionary<string, string> weather = new Dictionary<string, string>()
            {
                {"latitude",  (string)json["coord"]["lat"]},
                {"longitude", (string) json["coord"]["lon"]},
                {"geolocation", string.Join(";", (string)json["coord"]["lat"], (string) json["coord"]["lon"])},
                {"temperature", (string) json["main"]["temp"]},
                {"humidity", (string) json["main"]["humidity"]},
                {"pressure", (string) json["main"]["pressure"]},
                {"timezone", (string) json["timezone"]},
                {"weather", (string) json["weather"][0]["main"]}
            };
            return new ApiResponse(weather);
        }

        private void ValidCoordinates(string geolocation)
        {
            try
            {
                string[] coordinates = geolocation.Split(";");
                double longitude = double.Parse(coordinates[0]);
                double latitude = double.Parse(coordinates[1]);
                if (longitude < -180.0 || longitude > 180.0 ||
                     latitude < -90.0 || latitude > 90.0)
                    throw new Exception();
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid geolocation.");
            }
        }
    }
}
