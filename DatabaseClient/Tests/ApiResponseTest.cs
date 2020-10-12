using System.Collections.Generic;
using NUnit.Framework;

namespace DatabaseClient.Tests
{
    public class ApiResponseTest
    {
        static Dictionary<string, string> dict = new Dictionary<string, string>()
        {
            {"key", "value" }
        };
        ApiResponse test = new ApiResponse(dict);

        [Test]
        public void sizeTest()
        {
            Assert.AreEqual(1, test.size());
        }

        [Test]
        public void listTest()
        {
            string[] compare = { "key" };
            Assert.AreEqual(compare, test.list());
        }

        [Test]
        public void valuePositiveTest()
        {
            Assert.AreEqual("value", test.value("key"));
        }

        [Test]
        public void valueNegativeTest()
        {
            Assert.Throws<KeyNotFoundException>(() => test.value("not exhist"),
                "The given key 'not exhist' was not present in the dictionary.");
        }

        [Test]
        public void toStringTest()
        {
            Assert.AreEqual(test.ToString(), "{ \"key\": \"value\" }");
        }

        [Test]
        public void GetHashCodeTest()
        {
            Assert.Equals(100, dict.GetHashCode());
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
