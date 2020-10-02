using System;
using System.Collections.Generic;
using System.Text;
using Credentials;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.IO;
using System.Linq;

namespace ApiClients
{
    public class OpenWeatherMapClient
    {
        private RestClient _client;
        private string _key;
        private string _url = "/data/2.5/weather?";
        private static string _projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
        private static string _configPath = Path.Join(_projectPath, "WeatherAPI", "Configs", "ApiClientKeys.json");
        
        public OpenWeatherMapClient()
        {
            JsonFileContent config = new JsonFileContent(_configPath);
            _client = new RestClient((string)config.selectedParameter("OpenWeatherMapUrl"));
            _key = (string) config.selectedParameter("OpenWeatherMapKey"); 
        }

        public Weather apiRequest(string geolocation)
        {
            // Throws WebException if no internet.
            validInternetConnection();
            // Throws ArgumentException if coordinates are invalid.
            validCoordinates(geolocation);
            //Generates an HTTP request.
            var request = new RestRequest(_url, Method.GET);
            string[] coordinates = geolocation.Split(";");
            double longitude = double.Parse(coordinates[0]);
            double latitude = double.Parse(coordinates[1]);
            request.AddParameter("lat", latitude);
            request.AddParameter("lon", longitude);
            request.AddParameter("appid", _key);
            var response = _client.Execute(request);
            var content = response.Content;
            //
            JObject json = new JObject(response);
            Weather weather = new Weather(
                (string) json["lat"],
                (string) json["lat"],
                (string) json["current"]["temp"],
                (string) json["current"]["humidity"],
                (string) json["current"]["pressure"],
                (string) json["timezone"],
                (string) json["current"]["weather"]["main"]
                );
            return weather;
        }

        private void validInternetConnection()
        {
            WebClient web = new WebClient();
            try
            {
                web.DownloadData("http://www.google.com");
            }
            catch (WebException)
            {
                throw new WebException("Internet is not available.");
            }
        }

        private void validCoordinates(string geolocation)
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
