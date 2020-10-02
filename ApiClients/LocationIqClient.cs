using System;
using Credentials;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.IO;

namespace ApiClients
{
    public class LocationIqClient
    {
        private RestClient _client;
        private string _key;
        private string _url;
        private static string _projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
        private static string _configPath = Path.Join(_projectPath, "WeatherAPI", "Configs", "ApiClientKeys.json");

        public LocationIqClient()
        {
            JsonFileContent config = new JsonFileContent(_configPath);
            _client = new RestClient((string)config.selectedParameter("LocationIqUrl"));
            _key = (string)config.selectedParameter("LocationIqKey");
            _url = string.Format("/v1/search.php?");
        }

        public string apiRequest(string input)
        {
            // TODO: Check input in DB first
            // Throws WebException if no internet.
            validInternetConnection();
            //Generates an HTTP request.
            var request = new RestRequest(_url, Method.GET);
            string location = input.Trim();
            request.AddParameter("format", "json");
            request.AddParameter("q", location);
            request.AddParameter("key", _key);
            var response = _client.Execute(request);
            var content = response.Content;
            //
            if (locationNotExhist(content))
                return null;
            // Extracts coordinates.
            var contentArray = JArray.Parse(content);
            var coordinates = contentArray[0].ToString();
            var data = new JsonStringContent(coordinates);
            var lat = data.selectedParameter("lat").ToString();
            var lon = data.selectedParameter("lon").ToString();
            var result = String.Join(";", lat, lon);
            return result;
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

        private bool locationNotExhist(string response)
        {
            string notExhist = "Unable to geocode";
            if (response.Contains(notExhist))
                return true;
            return false;
        }
    }
}
