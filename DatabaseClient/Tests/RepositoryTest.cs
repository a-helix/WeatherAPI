using System.Collections.Generic;
using DatabaseClients;
using NUnit.Framework;

namespace DatabaseClients.Tests
{
    class RepositoryTest
    {
        public RepositoryTest() : base()
        {

        }
        
        private DatabaseEmulator _db;

        [SetUp]
        public void Setup()
        {
            _db = new DatabaseEmulator();
        }

        [Test]
        public void ReadTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"area", "Shire" }
            };
            ApiResponse test = new ApiResponse(dict);
            Assert.IsNull(_db.Read("Shire"));
            _db.Create(test);
            Assert.IsTrue(_db.Read("Shire").Equals(test));
            Assert.IsNull(_db.Read("Not exhist."));
        }

        [Test]
        public void CreateTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"area", "Shire" }
            };
            ApiResponse test = new ApiResponse(dict);
            Assert.IsFalse(_db.Contains("Shire"));
            _db.Create(test);
            Assert.IsTrue(_db.Contains("Shire"));
        }

        [Test]
        public void DeleteTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"area", "Delete" }
            };
            ApiResponse test = new ApiResponse(dict);
            Assert.IsFalse(_db.Contains("Delete"));
            _db.Create(test);
            Assert.IsTrue(_db.Contains("Delete"));
            _db.Delete("Delete");
            Assert.IsFalse(_db.Contains("Delete"));
            _db.Delete("Not exhist.");
            _db.Delete("Not exhist.");
        }

        [Test]
        public void UpdateTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"latitude",  "0" },
                {"longitude", "0" },
                {"geolocation", "0;0" },
                {"area", "Zero" },
            };
            ApiResponse test = new ApiResponse(dict);
            _db.Create(test);
            Assert.IsTrue(_db.Contains("Zero"));
            Assert.IsFalse(_db.Contains("null"));
            _db.Update("Zero", "null");
            Assert.IsFalse(_db.Contains("Zero"));
            Assert.IsTrue(_db.Contains("null"));
            Assert.Throws<KeyNotFoundException>(() => _db.Update("Not exhist.", "Still not exhist."));
        }
    }
}
