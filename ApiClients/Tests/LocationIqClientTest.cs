using NUnit.Framework;
using System;
using System.IO;
using Credentials;

namespace ApiClients.Tests
{
    
    public class LocationIqClientTest
    {
        static string configPath = Path.Combine(Directory.GetParent(
                                   Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                                   "WeatherAPI", "Configs", "ApiClientKeys.json");
        LocationIqClient client = new LocationIqClient(configPath);

        [Test]
        public void apiRequestPositiveTest()
        {
            var test = "40.7484284;-73.9856546198733";
            var request = "Empire State Building";
            var response = client.apiRequest(request);
            var compare = new JsonStringContent(response.json());
            Assert.AreEqual(compare.selectedParameter("geolocation"), test);
            Assert.AreEqual(compare.selectedParameter("area"), "New York:USA");
        }

        [Test]
        public void apiRequestNegativeTest()
        {
            var request = "0123456789qwerasdf";
            ApiResponse response;
            Assert.Throws<ArgumentException>(() => response = client.apiRequest(request), $"Unknown location { request }.");
        }
    }
}
