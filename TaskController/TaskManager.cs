using DatabaseClient;
using System;
using System.Collections.Generic;

namespace TaskController
{
    public class TaskManager
    {
        private MongoDatabaseClient _databaseClient;

        public TaskManager(MongoDatabaseClient databaseClient)
        {
            _databaseClient = databaseClient;
        }

        public enum RequestStatus
        {
            Created,
            Started,
            Finished,
            Aborted
        }

        public void Create(string userInput)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("area", userInput);
            parameters.Add("created", DateTime.UtcNow.Ticks.ToString());
            parameters.Add("active", true.ToString());
            parameters.Add("status", RequestStatus.Created.ToString());
            _databaseClient.Create(new ApiResponse(parameters));
        }

        public void UpdateStatus(string userInput, RequestStatus status)
        {
            var databaseValue = _databaseClient.Read(userInput);
            var parameters = new Dictionary<string, string>();
            parameters.Add("area", databaseValue.Value("area"));
            parameters.Add("created", databaseValue.Value("created"));
            parameters.Add("active", databaseValue.Value("active"));
            parameters.Add("status", status.ToString());
            _databaseClient.Delete(userInput);
            _databaseClient.Create(new ApiResponse(parameters));

        }

        public void Finish(string userInput)
        {
            var databaseValue = _databaseClient.Read(userInput);
            var parameters = new Dictionary<string, string>();
            parameters.Add("area", databaseValue.Value("area"));
            parameters.Add("created", databaseValue.Value("created"));
            parameters.Add("closed", DateTime.UtcNow.Ticks.ToString());
            parameters.Add("active", false.ToString());
            parameters.Add("status", RequestStatus.Finished.ToString());
            _databaseClient.Delete(userInput);
            _databaseClient.Create(new ApiResponse(parameters));
        }
    }
}
