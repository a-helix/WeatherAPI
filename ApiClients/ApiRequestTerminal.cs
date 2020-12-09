using DatabaseClient;
using Repository;
using System;

namespace ApiClients
{
    public class ApiRequestTerminal
    {
        private LocationIqClient _locationIqClient;
        private OpenWeatherMapClient _openWeatherMapClient;
        private IRepository<ApiResponse> _databaseAreaClient;

        public ApiRequestTerminal(string apiConfigPath, IRepository<ApiResponse> databaseAreaClient)
        {
            _locationIqClient = new LocationIqClient(apiConfigPath);
            _openWeatherMapClient = new OpenWeatherMapClient(apiConfigPath);
            _databaseAreaClient = databaseAreaClient;
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
