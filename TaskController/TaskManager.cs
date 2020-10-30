using ApiClients;
using Credentials;
using DatabaseClient;
using RabbitChat;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TaskController
{
    public class TaskManager : ITask
    {
        private static string _configPath;
        private static Dictionary<string, ApiResponse> _cache;
        private static MongoDatabaseClient dbClient;
        private static Publisher _publisher;

        public TaskManager(string configPath, Publisher publisher)
        {
            _cache = new Dictionary<string, ApiResponse>();
            var configs = new JsonFileContent(configPath);
            var databaseUrl = (string) configs.Parameter("databaseUrl");
            dbClient = new MongoDatabaseClient(databaseUrl, "Tasks", "tasks");
            _configPath = configPath;
            _publisher = publisher;
        }

        private static void UpdateCacheAndDatabase(string input, ApiResponse newResponse)
        {
            _cache.Remove(input);
            TaskManager._cache.Add(input, newResponse);
            TaskManager.dbClient.Delete(input);
            TaskManager.dbClient.Create(newResponse);
        }

        public void Execute(string input)
        {
            var request = new TaskCreate();
            request.Execute(input);

        }

        public class TaskCreate : ITask
        {
            private Dictionary<string, string> _buffer;
            public void Execute(string input)
            {
                try
                {
                    _buffer = new Dictionary<string, string>();
                    _buffer.Add("area", input);
                    _buffer.Add("created", DateTime.UtcNow.Ticks.ToString());
                    _buffer.Add("geolocation", input);
                    _buffer.Add("status", "Created");
                    var created = new ApiResponse(_buffer);
                    TaskManager.dbClient.Create(created);
                    TaskManager._cache.Add(input, created);
                }
                catch(Exception e)
                {
                    TaskCancel cancel = new TaskCancel(e);
                    cancel.Execute(input);
                }
            }
        }

        public class TaskRequest : ITask
        {
            ApiRequestTerminal terminal = new ApiRequestTerminal(_configPath);
            private Dictionary<string, string> _buffer;
            TaskPush push;
            public void Execute(string input)
            {
                var cached = _cache[input];
                push = new TaskPush();

                try
                {
                    Regex rx = new Regex(@"((((\-?\d{1,3}\.\d+)|\-?\d{1,3}))\;((\-?\d{1,2}\.\d+)|(\-?\d{1,2})))");
                    ApiResponse apiResponse;
                    if (rx.IsMatch(input))
                    {
                        apiResponse = terminal.Execute(input ,"coordinates");
                        apiResponse.Add("area", input);
                    }
                    else
                    {
                        apiResponse = terminal.Execute(input, "location");
                        apiResponse.Update("area", input);
                    }
                    apiResponse.Add("status", "Requested");
                    apiResponse.Add("created", cached.Value("created"));
                    apiResponse.Add("finished",DateTime.UtcNow.Ticks.ToString());
                    TaskManager.UpdateCacheAndDatabase(input, cached);
                    push.Execute(input);
                }
                catch(Exception e)
                {
                    TaskCancel cancel = new TaskCancel(e);
                    cancel.Execute(input);
                }
            }
        }

        public class TaskPush : ITask
        {
            private TaskComplete _complete;
            public void Execute(string input)
            {
                try
                {
                    var cached = _cache[input];
                    cached.Update("status", "Published");
                    _publisher.Send(input, cached.ToString());
                    TaskManager.UpdateCacheAndDatabase(input, cached);
                    _complete = new TaskComplete();
                    _complete.Execute(input);
                    
                }
                catch(Exception e)
                {
                    TaskCancel cancel = new TaskCancel(e);
                    cancel.Execute(input);
                }
                
            }
        }

        public class TaskComplete : ITask
        {
            public void Execute(string input)
            {
                var cached = _cache[input];
                cached.Update("status", "Completed");
                TaskManager.UpdateCacheAndDatabase(input, cached);
            }
        }


        public class TaskCancel : ITask
        {
            private Exception _exception;
            public TaskCancel(Exception exception)
            {
                _exception = exception;
            }
            public void Execute(string input)
            {
                var cached = _cache[input];
                
                cached.Update("status", "Canceled");
                cached.Add("error", _exception.ToString());
                TaskManager.UpdateCacheAndDatabase(input, cached);

            }
        }
    }
}
