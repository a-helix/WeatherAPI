using NUnit.Framework;
using System.IO;

namespace ApiClients.Tests
{
    
    public class LocationIqClientTest
    {

        LocationIqClient client = new LocationIqClient();

        [Test]
        public void apiRequestPositiveTest()
        {
            string test = "40.7484284;-73.9856546198733";
            string request = "Empire State Building";
            string response = client.apiRequest(request);
            Assert.AreEqual(response, test);
        }

        [Test]
        public void apiRequestNegativeTest()
        {
            string test = null;
            string request = "0123456789qwerasdf";
            string response = client.apiRequest(request);
            Assert.AreEqual(response, test);
        }
    }
}
