using NUnit.Framework;
using Credentials;
using RabbitChat;

namespace ApiClients.Tests
{
    
    class LocationIqClientTest
    {
        [Test]
        public void apiRequestPositiveTest()
        {
            LocationIqClient client = new LocationIqClient();
            JsonStringContent json = new JsonStringContent("LocationIqTest.json");
            string request = "Empire State Building";
            client.apiRequest(request);
            Consumer consumer = new Consumer("localhost", "LocationIqClientTest", "LocationIqClientTest");
            string result = consumer.receive(request);
            Assert.AreEqual(result, json);
        }

        [Test]
        public void apiRequestNegativeTest()
        {
            LocationIqClient client = new LocationIqClient();
            JsonStringContent json = new JsonStringContent("LocationIqTest.json");
            string request = "In the middle of nowhere";
            client.apiRequest(request);
            Assert.AreEqual(request, null);
        }
    }
}
