using NUnit.Framework;
using System;

namespace ApiClients.Tests
{
    
    public class LocationIqClientTest
    {
        LocationIqClient client = new LocationIqClient();

        [Test]
        public void apiRequestPositiveTest()
        {
            var test = "40.7484284;-73.9856546198733";
            var request = "Empire State Building";
            var response = client.apiRequest(request);
            Assert.AreEqual(response, test);
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
