using DatabaseClient;
using System;

namespace ApiClients
{
    public class ApiRequestTerminal
    {
        private LocationIqClient _locationIqClient;
        private OpenWeatherMapClient _openWeatherMapClient;

    public ApiRequestTerminal(string apiConfigPath)
        {
        _locationIqClient = new LocationIqClient(apiConfigPath);
        _openWeatherMapClient = new OpenWeatherMapClient(apiConfigPath);
        }

    public ApiResponse Execute(string parameter, string location)
        {
            switch (parameter)
            {
                case "coordinates":
                    return _openWeatherMapClient.ApiRequest(location);
                case "location":
                    // Use the name of a city like "New York:New York County:USA"
                    string geolocation = _locationIqClient.ApiRequest(location).Value("geolocation");
                    return _openWeatherMapClient.ApiRequest(geolocation);
                default:
                    throw new ArgumentException($"Invalid argument {parameter}.");
            }
        }
    }
}
