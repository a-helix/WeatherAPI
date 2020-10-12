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
        public void apiRequestPositiveTest()
        {
            var latitude = "40.75";
            var longitude = "-73.99";
            var timezone = "-14400";
            var coordinates = String.Join(";", latitude, longitude);
            ApiResponse feedback = client.apiRequest(coordinates);
            Assert.AreEqual(latitude, feedback.value("latitude"));
            Assert.AreEqual(longitude, feedback.value("longitude"));
            Assert.AreEqual(timezone, feedback.value("timezone"));
        }

        [Test]
        public void apiRequestNegativeTest()
        {
            var latitude = "Unknown";
            var longitude = "Unknown";
            var coordinates = String.Join(";", latitude, longitude);
            
            Assert.Throws<ArgumentException> (() => client.apiRequest(coordinates), "Invalid geolocation.");
        }
    }
}
