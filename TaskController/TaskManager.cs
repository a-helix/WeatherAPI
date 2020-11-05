using ApiClients;
using DatabaseClients;
using RabbitChat;
using Repository;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TaskController
{
    public class TaskManager : ITask
    {
        protected static ApiRequestTerminal _terminal;
        protected static IRepository<ApiResponse> _dbClient;
        protected static IPublisher _publisher;

        public TaskManager(ApiRequestTerminal terminal, IPublisher publisher, IRepository<ApiResponse> databaseClient)
        {
            _terminal = terminal;
            _dbClient = databaseClient;
            _publisher = publisher;
        }

        public void Execute(string input)
        {
            var request = new TaskCreate();
            request.Execute(input);
        }

        public abstract class Task : ITask
        {
            public void Execute(string input)
            {
                throw new NotImplementedException();
            }

            protected void UpdateCacheAndDatabase(string input, ApiResponse newResponse)
            {
                TaskBuffer.Delete(input);
                TaskBuffer.Add(input, newResponse);
                _dbClient.Delete(input);
                _dbClient.Create(newResponse);
            }
        }

        public class TaskCreate : Task
        {
            private Dictionary<string, string> _buffer;
            TaskRequest request;
            public new void Execute(string input)
            {
                try
                {
                    _buffer = new Dictionary<string, string>();
                    _buffer.Add("area", input);
                    _buffer.Add("created", DateTime.UtcNow.Ticks.ToString());
                    _buffer.Add("geolocation", input);
                    _buffer.Add("status", "Created");
                    var created = new ApiResponse(_buffer);
                    _dbClient.Create(created);
                    TaskBuffer.Add(input, created);
                    request = new TaskRequest();
                    request.Execute(input);
            }
                catch (Exception e)
                {
                    TaskCancel cancel = new TaskCancel(e);
                    cancel.Execute(input);
                }
    }
        }

        public class TaskRequest : Task
        {
            TaskPush push;

            public new void Execute(string input)
            {
                var cached = TaskBuffer.Get(input);
                push = new TaskPush();
                try
                {
                    Regex rx = new Regex(@"((((\-?\d{1,3}\.\d+)|\-?\d{1,3}))\;((\-?\d{1,2}\.\d+)|(\-?\d{1,2})))");
                    ApiResponse apiResponse;

                    if (rx.IsMatch(input))
                    {
                        apiResponse = _terminal.Execute("coordinates", input);
                        apiResponse.Add("area", input);
                    }
                    else
                    {
                        apiResponse = _terminal.Execute("location", input);
                        apiResponse.Update("area", input);
                    }

                    apiResponse.Update("status", "Requested");
                    apiResponse.Update("created", cached.Value("created"));

                apiResponse.Add("finished", DateTime.UtcNow.Ticks.ToString());
                UpdateCacheAndDatabase(input, cached);
                push.Execute(input);
                }
                catch (Exception e)
                {
                    TaskCancel cancel = new TaskCancel(e);
                    cancel.Execute(input);
                }
            }
        }

        public class TaskPush : Task
        {
            private TaskComplete _complete;
            public new void Execute(string input)
            {
                try
                {
                    var cached = TaskBuffer.Get(input);
                    cached.Update("status", "Published");
                    _publisher.SendQueue(input, cached.ToString());
                    UpdateCacheAndDatabase(input, cached);
                    _complete = new TaskComplete();
                    _complete.Execute(input);
                }
                catch (Exception e)
                {
                    TaskCancel cancel = new TaskCancel(e);
                    cancel.Execute(input);
                }
            }
        }

        public class TaskComplete : Task
        {
            public new void Execute(string input)
            {
                var cached = TaskBuffer.Get(input);
                cached.Update("status", "Completed");
                UpdateCacheAndDatabase(input, cached);
            }
        }

        public class TaskCancel : Task
        {
            private Exception _exception;

            public TaskCancel(Exception exception)
            {
                _exception = exception;
            }
            public new void Execute(string input)
            {
                var cached = TaskBuffer.Get(input);
                cached.Update("status", "Canceled");
                cached.Add("error", _exception.ToString());
                UpdateCacheAndDatabase(input, cached);
            }
        }
    }
}
