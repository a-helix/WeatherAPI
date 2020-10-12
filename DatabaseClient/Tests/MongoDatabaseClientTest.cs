using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Credentials;
using DatabaseClient;
using NUnit.Framework;

namespace DatabaseClients.Tests
{
    class MongoDatabaseClientTest
    {
        static string databaseConfigPath = Path.Combine(Directory.GetParent(
                                   Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                                   "WeatherAPI", "Configs", "ApiConfigs.json");
        static JsonFileContent config = new JsonFileContent(databaseConfigPath);
        MongoDatabaseClient testClient = new MongoDatabaseClient((string) config.selectedParameter("databaseUrl"), "test");

        [Test]
        public void ContainsTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"area", "Shire" }
            };
            ApiResponse test = new ApiResponse(dict);
            Assert.IsFalse(testClient.Contains("Shire"));
            testClient.Create(test);
            Assert.IsTrue(testClient.Contains("Shire"));
        }

        [Test]
        public void CreatePositieTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"area", "Shire" }
            };
            ApiResponse test = new ApiResponse(dict);
            Assert.IsFalse(testClient.Contains("Shire"));
            testClient.Create(test);
            Assert.IsTrue(testClient.Contains("Shire"));
        }

        [Test]
        public void DeletePositieTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"area", "Delete" }
            };
            ApiResponse test = new ApiResponse(dict);
            testClient.Create(test);
            Assert.IsTrue(testClient.Contains("Delete"));
            testClient.Delete("Delete");
            Assert.IsFalse(testClient.Contains("Delete"));
        }

        [Test]
        public void DeleteNegativeTest()
        {
            testClient.Delete("Not exhist.");
            Assert.IsTrue(false);
        }

        [Test]
        public void GetPositieTest()
        {

        }

        [Test]
        public void GetNegativeTest()
        {
            testClient.Get("Who knows where.");
            Assert.IsTrue(false);
        }

        [Test]
        public void UpdatePositieTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"area", "Zero" },
                {"geolocation", "0;0" }
            };
            ApiResponse test = new ApiResponse(dict);
            testClient.Create(test);

        }

        [Test]
        public void UpdateNegativeTest()
        {
            testClient.Update("Not exhist.", "Still not exhist.");
            Assert.IsTrue(false);
        }
    }
}
