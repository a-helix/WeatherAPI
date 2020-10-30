using Credentials;
using DatabaseClient;
using System;

namespace ApiClients
{
    public class ApiRequestTerminal
    {
        private LocationIqClient _locationIqClient;
        private OpenWeatherMapClient _openWeatherMapClient;
        private MongoDatabaseClient _databaseAreaClient;

        public ApiRequestTerminal(string apiConfigPath)
        {
            _locationIqClient = new LocationIqClient(apiConfigPath);
            _openWeatherMapClient = new OpenWeatherMapClient(apiConfigPath);
            var _configs = new JsonFileContent(apiConfigPath);
            var databaseUrl = (string)_configs.Parameter("databaseUrl");
            _databaseAreaClient = new MongoDatabaseClient(databaseUrl, "Areas", "areas");
        }

    public ApiResponse Execute(string parameter, string location)
        {
            switch (parameter)
            {
                case "coordinates":
                    return _openWeatherMapClient.ApiRequest(location);
                case "location":
                    var dbResponse = _databaseAreaClient.Read(location);
                    if (dbResponse != null)
                    {
                        return dbResponse;
                    }
                    // Use the name of a city like "New York:New York County:USA"
                    string geolocation = _locationIqClient.ApiRequest(location).Value("geolocation");
                    return _openWeatherMapClient.ApiRequest(geolocation);
                default:
                    throw new ArgumentException($"Invalid argument {parameter}.");
            }
        }
    }
}
