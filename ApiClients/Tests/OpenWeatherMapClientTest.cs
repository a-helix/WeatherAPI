using DatabaseClient;
using NUnit.Framework;
using System;
using System.IO;

namespace ApiClients.Tests
{
    class OpenWeatherMapClientTest
    {
        static string configPath = Path.Combine(Directory.GetParent(
                                   Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, 
                                   "WeatherAPI", "Configs", "ApiConfigs.json");
        OpenWeatherMapClient client = new OpenWeatherMapClient(configPath);

        [Test]
        public void ApiRequestTest()
        {
            var latitude = "40.75";
            var longitude = "-73.99";
            var timezone = "-18000";
            var coordinates = String.Join(";", latitude, longitude);
            ApiResponse feedback = client.ApiRequest(coordinates);
            Assert.AreEqual(latitude, feedback.Value("latitude"));
            Assert.AreEqual(longitude, feedback.Value("longitude"));
            Assert.AreEqual(timezone, feedback.Value("timezone"));

            var lat = "Unknown";
            var lon = "Unknown";
            var coord = String.Join(";", lat, lon);
            Assert.Throws<ArgumentException>(() => client.ApiRequest(coord), "Invalid geolocation.");
        }
    }
}
