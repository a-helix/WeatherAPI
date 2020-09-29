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
            var coordinates = String.Join(latitude, longitude, ";");
            client.apiRequest(coordinates);
            Consumer consumer = new Consumer("localhost", "LocationIqClientTest", "LocationIqClientTest");
            var result = consumer.receive(coordinates);
            var feedback = new JsonStringContent(result);
            Assert.AreEqual(latitude, (string) feedback.selectedParameter("lat"));
            Assert.AreEqual(longitude, (string) feedback.selectedParameter("lon"));
        }

        [Test]
        public void apiRequestNegativeTest()
        {
            OpenWeatherMapClient client = new OpenWeatherMapClient();
            var latitude = "Unknown";
            var longitude = "Unknown";
            var coordinates = String.Join(latitude, longitude, ";");
            client.apiRequest(coordinates);
            Assert.AreEqual(latitude, "Have no idea what's the feedback.");

        }
    }
}
