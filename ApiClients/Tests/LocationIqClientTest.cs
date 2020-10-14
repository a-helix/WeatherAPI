using NUnit.Framework;
using System;
using System.IO;
using Credentials;
using DatabaseClient;

namespace ApiClients.Tests
{
    
    public class LocationIqClientTest
    {
        static string configPath = Path.Combine(Directory.GetParent(
                                   Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                                   "WeatherAPI", "Configs", "ApiConfigs.json");
        LocationIqClient client = new LocationIqClient(configPath);

        [Test]
        public void ApiRequestPositiveTest()
        {
            var test = "40.7484284;-73.9856546198733";
            var request = "Empire State Building";
            var response = client.ApiRequest(request);
            var compare = new JsonStringContent(response.ToString());
            Assert.AreEqual(compare.Parameter("geolocation"), test);
            Assert.AreEqual(compare.Parameter("area"), "New York:New York County:USA");
        }

        [Test]
        public void ApiRequestNegativeTest()
        {
            var request = "0123456789qwerasdf";
            ApiResponse response;
            Assert.Throws<ArgumentException>(() => response = client.ApiRequest(request), $"Unknown location { request }.");
        }
    }
}
