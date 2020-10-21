using NUnit.Framework;
using System;
using Credentials;
using DatabaseClient;
using System.IO;

namespace ApiClients.Tests
{
    
    public class LocationIqClientTest
    {
        static string configPath = Path.Combine(Directory.GetParent(
                                   Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                                   "WeatherAPI", "Configs", "ApiConfigs.json");
        LocationIqClient client = new LocationIqClient(configPath);

        [Test]
        public void ApiRequestTest()
        {
            var test = "40.7484284;-73.9856546198733";
            var request = "Empire State Building";
            var response = client.ApiRequest(request);
            var compare = new JsonStringContent(response.ToString());
            Assert.AreEqual(compare.Parameter("geolocation"), test);
            Assert.AreEqual(compare.Parameter("area"), "New York:New York County:USA");

            var negativeRequest = "0123456789qwerasdf";
            ApiResponse negativeResponse;
            Assert.Throws<ArgumentException>(() => negativeResponse = client.ApiRequest(negativeRequest), $"Unknown location { negativeRequest }.");
        }
    }
}
