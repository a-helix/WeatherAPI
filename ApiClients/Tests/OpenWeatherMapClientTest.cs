using NUnit.Framework;
using System;

namespace ApiClients.Tests
{
    class OpenWeatherMapClientTest
    {
        [Test]
        public void apiRequestPositiveTest()
        {
            OpenWeatherMapClient client = new OpenWeatherMapClient();
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
            OpenWeatherMapClient client = new OpenWeatherMapClient();
            var latitude = "Unknown";
            var longitude = "Unknown";
            var coordinates = String.Join(";", latitude, longitude);
            
            Assert.Throws<ArgumentException> (() => client.apiRequest(coordinates), "Invalid geolocation.");
        }
    }
}
