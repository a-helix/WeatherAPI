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
        public void ContainsPositieTest()
        {

        }

        [Test]
        public void ContainsNegativeTest()
        {

        }

        [Test]
        public void CreatePositieTest()
        {

        }

        //[Test]
        //public void CreateNegativeTest()
        //{

        //}

        [Test]
        public void DeletePositieTest()
        {

        }

        [Test]
        public void DeleteNegativeTest()
        {

        }

        [Test]
        public void GetPositieTest()
        {

        }

        [Test]
        public void GetNegativeTest()
        {

        }

        [Test]
        public void UpdatePositieTest()
        {

        }

        [Test]
        public void UpdateNegativeTest()
        {

        }
    }
}
