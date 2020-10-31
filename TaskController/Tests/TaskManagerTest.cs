using ApiClients;
using DatabaseClients;
using Repository;
using RabbitChat;
using NUnit.Framework;
using System.IO;
using DatabaseClients.Tests;

namespace TaskController.Tests
{
    public class TaskManagerTest
    {
        static RabbitServerEmulator rabbitServer = new RabbitServerEmulator();
        TaskManager taskManager;
        static string configPath = Path.Combine(Directory.GetParent(
                                   Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                                   "WeatherAPI", "Configs", "ApiConfigs.json");
        PublisherEmulator publisher = new PublisherEmulator(rabbitServer, "test", "test");
        static DatabaseEmulator databaseClient = new DatabaseEmulator();
        ApiRequestTerminal terminal = new ApiRequestTerminal(configPath, databaseClient);
        ConsumerEmulator consumer = new ConsumerEmulator(rabbitServer, "test", "test");

        [SetUp]
        public void SetUp()
        {
            taskManager = new TaskManager(terminal, publisher, databaseClient); 
        }

        [Test]
        public void ExecuteTest()
        {
            string positiveTest = "Empire State Building:New York:USA";
            taskManager.Execute(positiveTest);
            string positiveEtalon = "positive";
            string positiveFeedback = consumer.Receive(positiveTest);
            Assert.AreEqual(positiveFeedback, positiveEtalon);
            string negativeTest = "1234567890qwerasdf";
            taskManager.Execute(negativeTest);
            string negativeEtalon = "negative";
            string negativeFeedback = consumer.Receive(negativeTest);
            Assert.AreEqual(negativeFeedback, negativeEtalon);
        }

    }
}

