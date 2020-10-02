using NUnit.Framework;
using Credentials;
using RabbitChat;
using System;

namespace ApiClients.Tests
{
    class OpenWeatherMapClientTest
    {
        [Test]
        public void apiRequestPositiveTest()
        {
            OpenWeatherMapClient client = new OpenWeatherMapClient();
            var latitude = "40.7484284";
            var longitude = "-73.9856546198733";
            var coordinates = String.Join(";", latitude, longitude);
            Weather feedback = client.apiRequest(coordinates);
            Assert.AreEqual(latitude, feedback.latitude);
            Assert.AreEqual(longitude, feedback.longitude);
        }

        //[Test]
        //public void apiRequestNegativeTest()
        //{
        //    OpenWeatherMapClient client = new OpenWeatherMapClient();
        //    var latitude = "Unknown";
        //    var longitude = "Unknown";
        //    var coordinates = String.Join(latitude, longitude, ";");
        //    client.apiRequest(coordinates);
        //    Assert.AreEqual(latitude, "Have no idea what's the feedback.");
        //}
    }
}
