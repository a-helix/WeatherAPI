using System.Collections.Generic;
using NUnit.Framework;

namespace ApiClients.Tests
{
    public class ApiResponseTest
    {
        static Dictionary<string, string> dict = new Dictionary<string, string>()
        {
            {"key", "value" }
        };
        ApiResponse test = new ApiResponse(dict);

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
    }
}
