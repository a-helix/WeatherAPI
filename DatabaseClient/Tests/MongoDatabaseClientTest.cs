using System;
using System.Collections.Generic;
using System.IO;
using Credentials;
using DatabaseClient;
using NUnit.Framework;

namespace DatabaseClients.Tests
{
    public class MongoDatabaseClientTest : MongoDatabaseClient
    {
        public MongoDatabaseClientTest(string configPath, string database, string collection)
                                     : base(configPath, database, collection)
        {

        }

        static string databaseConfigPath = Path.Combine(Directory.GetParent(
                                   Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                                   "WeatherAPI", "Configs", "ApiConfigs.json");
        static JsonFileContent config = new JsonFileContent(databaseConfigPath);
        static string databaseUrl = (string) config.Parameter("databaseUrl");
        private MongoDatabaseClientTest testClient = new MongoDatabaseClientTest(databaseUrl, "test", "test");



        

        private bool Contains(string location)
        {
            try
            {
                var test = Get(location);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Test]
        public void ContainsNegativeTest()
        {
            Assert.IsFalse(testClient.Contains("Shire"));
        }

        [Test]
        public void ContainsPositiveTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"area", "Shire" }
            };
            ApiResponse test = new ApiResponse(dict);
            Assert.IsFalse(testClient.Contains("Shire"));
            testClient.Insert(test);
            Assert.IsTrue(testClient.Contains("Shire"));
        }

        [Test]
        public void InsertPositieTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"area", "Shire" }
            };
            ApiResponse test = new ApiResponse(dict);
            Assert.IsFalse(testClient.Contains("Shire"));
            testClient.Insert(test);
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
            testClient.Insert(test);
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
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"area", "GetPositiveTest" }
            };
            ApiResponse test = new ApiResponse(dict);
            testClient.Insert(test);
            ApiResponse compare = testClient.Get("GetPositiveTest");
            Assert.IsTrue(test.Equals(compare));
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
            testClient.Insert(test);

        }

        [Test]
        public void UpdateNegativeTest()
        {
            testClient.Update("Not exhist.", "Still not exhist.");
            Assert.IsTrue(false);
        }
    }
}
