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

    public ApiResponse execute(string parameter, string location)
        {
            switch (parameter)
            {
                case "coordinates":
                    return _openWeatherMapClient.apiRequest(location);
                case "location":
                    // Use the name of a city like "New York:New York County:USA"
                    string geolocation = _locationIqClient.apiRequest(location).value("geolocation");
                    return _openWeatherMapClient.apiRequest(geolocation);
                default:
                    throw new ArgumentException($"Invalid argument {parameter}.");
            }
        }
    }
}
