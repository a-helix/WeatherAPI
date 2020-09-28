using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Credentials;
using System;
using Microsoft.VisualBasic.CompilerServices;


namespace ApiClients.Tests
{
    class LocationIqClientTest
    {
        [Test]
        public void selectedParameterPositiveTest()
        {
            LocationIqClient client = new LocationIqClient();
            JsonStringContent json = new JsonStringContent("LocationIq.json");
            string test = client.apiRequest("Empire State Building");
            Assert.AreEqual(test.selectedParameter("family_name"), "Escobar");
        }
    }
}
