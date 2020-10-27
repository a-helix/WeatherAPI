using Credentials;
using DatabaseClient;
using System;
using TaskController;

namespace ApiClients
{
    public class ApiRequestTerminal
    {
        private LocationIqClient _locationIqClient;
        private OpenWeatherMapClient _openWeatherMapClient;
        private MongoDatabaseClient _databaseAreaClient;
        private MongoDatabaseClient _databaseTaskClient;
        private TaskManager _taskManager;

        public ApiRequestTerminal(string apiConfigPath)
        {
            _locationIqClient = new LocationIqClient(apiConfigPath);
            _openWeatherMapClient = new OpenWeatherMapClient(apiConfigPath);
            var _configs = new JsonFileContent(apiConfigPath);
            var databaseUrl = (string)_configs.Parameter("databaseUrl");
            _databaseAreaClient = new MongoDatabaseClient(databaseUrl, "Areas", "areas");
            _databaseTaskClient = new MongoDatabaseClient(databaseUrl, "Tasks", "tasks");
            _taskManager = new TaskManager(_databaseTaskClient);
        }

    public ApiResponse Execute(string parameter, string location)
        {
            _taskManager.Create(location);
            _taskManager.UpdateStatus(location, TaskManager.RequestStatus.Started);
            switch (parameter)
            {
                case "coordinates":
                    return _openWeatherMapClient.ApiRequest(location);
                case "location":
                    var dbResponse = _databaseAreaClient.Read(location);
                    if (dbResponse != null)
                    {
                        _taskManager.Finish(location);
                        return dbResponse;
                    }
                    // Use the name of a city like "New York:New York County:USA"
                    string geolocation = _locationIqClient.ApiRequest(location).Value("geolocation");
                    _taskManager.Finish(location);
                    return _openWeatherMapClient.ApiRequest(geolocation);
                default:
                    _taskManager.UpdateStatus(location, TaskManager.RequestStatus.Aborted);
                    throw new ArgumentException($"Invalid argument {parameter}.");
            }
        }
    }
}
