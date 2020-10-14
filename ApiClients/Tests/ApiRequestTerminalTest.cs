using System;
using System.IO;
using NUnit.Framework;
using Credentials;
using DatabaseClient;

namespace ApiClients.Tests
{
    class ApiRequestTerminalTest
    {
        static string configPath = Path.Combine(Directory.GetParent(
                                   Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                                   "WeatherAPI", "Configs", "ApiConfigs.json");
        ApiRequestTerminal terminal = new ApiRequestTerminal(configPath);

        [Test]
        public void ExecutePositiveTest()
        {
            var response = terminal.Execute("coordinates", "40.75;-73.99");
            var result = new JsonStringContent(response.ToString());
            Assert.AreEqual(result.Parameter("geolocation"), "40.75;-73.99");
            Assert.AreEqual(result.Parameter("timezone"), "-14400");
        }

        [Test]
        public void ExecuteNegativeTest()
        {
            ApiResponse response;
            Assert.Throws<ArgumentException>(() => response = terminal.Execute("not exhist", "40.75;-73.99"),
                "Invalid argument not exhist.");
        }
    }
}
