using System;
using System.IO;
using NUnit.Framework;
using Credentials;

namespace ApiClients.Tests
{
    class ApiRequestTerminalTest
    {
        static string configPath = Path.Combine(Directory.GetParent(
                                   Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                                   "WeatherAPI", "Configs", "ApiClientKeys.json");
        ApiRequestTerminal terminal = new ApiRequestTerminal(configPath);

        [Test]
        public void executePositiveTest()
        {
            var response = terminal.execute("coordinates", "40.75;-73.99");
            var result = new JsonStringContent(response.json());
            Assert.AreEqual(result.selectedParameter("geolocation"), "40.75;-73.99");
            Assert.AreEqual(result.selectedParameter("timezone"), "-14400");
        }

        [Test]
        public void executeNegativeTest()
        {
            ApiResponse response;
            Assert.Throws<ArgumentException>(() => response = terminal.execute("not exhist", "40.75;-73.99"),
                "Invalid argument not exhist.");
        }
    }
}
