﻿using ApiClients;
using Credentials;
using DatabaseClient;
using RabbitChat;
using Repository;

namespace TaskController
{
    public class WeatherApiController
    {
        TaskManager manager;
        Publisher publisher;
        ApiRequestTerminal terminal;
        Consumer consumer;
        string rabbitExchangeValue;

        public WeatherApiController(string configPath, IRepository<ApiResponse> databaseClient)
        {
            JsonFileContent config = new JsonFileContent(configPath);
            var rabbitAdress = (string)config.Value("RabbitMQ");
            publisher = new Publisher(rabbitAdress, "TaskController", "TaskController");
            consumer = new Consumer(rabbitAdress, "TaskController", "TaskController");
            terminal = new ApiRequestTerminal(configPath, databaseClient);
            manager = new TaskManager(terminal, publisher, databaseClient);
            rabbitExchangeValue = (string)config.Value("QueueKey"); 
        }

        public void Run()
        {
            string feedback;
            while(true)
            {
                //Error here
                feedback = consumer.ReceiveQueue(rabbitExchangeValue);
                if(feedback.Equals(null))
                {
                    continue;
                }
                manager.Execute(feedback);
            }
        }
    }
}
