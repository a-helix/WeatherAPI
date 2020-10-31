using System.Collections.Generic;
using NUnit.Framework;

namespace DatabaseClients.Tests
{
    public class ApiResponseTest
    {
        static Dictionary<string, string> dict;
        ApiResponse test;

        [SetUp]
        public void SetUp()
        {

            dict = new Dictionary<string, string>()
            {
                {"key", "value" }
            };
            test = new ApiResponse(dict);
        }

        [Test]
        public void SizeTest()
        {
            Assert.AreEqual(1, test.Size());
        }

        [Test]
        public void ListTest()
        {
            string[] compare = { "key" };
            Assert.AreEqual(compare, test.List());
        }

        [Test]
        public void UpdateTest()
        {
            test.Add("control", "before");
            Assert.AreEqual(test.Value("control"), "before");
            test.Update("control", "after");
            Assert.AreEqual(test.Value("control"), "after");
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual(test.ToString(), "{\r\n  \"key\": \"value\"\r\n}");
        }

        [Test]
        public void AddTest()
        {
            string value = "exhist";
            test.Add("control", value);
            Assert.AreEqual(test.Value("control"), value);
        }

        [Test]
        public void ValuePositiveTest()
        {
            Assert.AreEqual("value", test.Value("key"));
        }

        [Test]
        public void ValueNegativeTest()
        {
            Assert.Throws<KeyNotFoundException>(() => test.Value("not exhist"),
                "The given key 'not exhist' was not present in the dictionary.");
        }

        [Test]
        public void GetHashCodeTest()
        {
            Assert.AreEqual(62407605, dict.GetHashCode());
        }

        [Test]
        public void EqualsPositiveTest()
        {
            ApiResponse compare = new ApiResponse(dict);
            Assert.IsTrue(test.Equals(compare));
        }

        [Test]
        public void EqualsNegativeTest()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"Hello", "World" }
            };
            ApiResponse compare = new ApiResponse(dict);
            Assert.IsFalse(test.Equals(compare));
        }
    }
}
