using ApiClients;
using RabbitChat;
using NUnit.Framework;
using System.IO;
using DatabaseClients.Tests;
using System.Collections.Generic;
using DatabaseClients;
using Credentials;

namespace TaskController.Tests
{
    public class TaskManagerTest
    {
        static RabbitServerEmulator rabbitServer = new RabbitServerEmulator();
        static string configPath = Path.Combine(Directory.GetParent(
                                   Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                                   "WeatherAPI", "Configs", "ApiConfigs.json");
        static PublisherEmulator publisher = new PublisherEmulator(rabbitServer, "test", "test");
        static DatabaseEmulator databaseClient = new DatabaseEmulator();
        static ApiRequestTerminal terminal = new ApiRequestTerminal(configPath, databaseClient);
        ConsumerEmulator consumer = new ConsumerEmulator(rabbitServer, "test", "test");
        TaskManager taskManager = new TaskManager(terminal, publisher, databaseClient);

        [Test]
        public void ExecuteTest()
        {
            var positive = new Dictionary<string, string>(){
                 { "area", "Empire State Building:New York:USA" }, 
                 { "created", "637399363231592838" }, 
                 { "geolocation", "Empire State Building:New York:USA" }, 
                 { "status", "Published" }, 
                 { "finished", "637399363231723630" }
            };
            string positiveTest = "Empire State Building:New York:USA";
            taskManager.Execute(positiveTest);
            var positiveEtalon = new ApiResponse(positive);
            var positiveFeedback = new JsonStringContent(consumer.Receive(positiveTest));
            Assert.AreEqual(positiveFeedback.Value("area"), positiveEtalon.Value("area"));
            Assert.AreEqual(positiveFeedback.Value("geolocation"), positiveEtalon.Value("geolocation"));
            Assert.AreEqual(positiveFeedback.Value("status"), positiveEtalon.Value("status"));

            var negative = new Dictionary<string, string>(){
                 { "area", "1234567890qwerasdf" },
                 { "created", "637399363231592838" },
                 { "geolocation", "1234567890qwerasdf" },
                 { "status", "Published" },
                 { "finished", "637399363231723630" }
            };
            string negativeTest = "1234567890qwerasdf";
            taskManager.Execute(negativeTest);
            var negativeEtalon = new ApiResponse(negative);
            var negativeFeedback = new JsonStringContent(consumer.Receive(negativeTest));
            Assert.AreEqual(negativeFeedback.Value("area"), negativeEtalon.Value("area"));
            Assert.AreEqual(negativeFeedback.Value("geolocation"), negativeEtalon.Value("geolocation"));
            Assert.AreEqual(negativeFeedback.Value("status"), negativeEtalon.Value("status"));
        }

    }
}

