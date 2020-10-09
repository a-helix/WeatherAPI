using System;

namespace ApiClients
{
    public class ApiRequestTerminal
    {
        private LocationIqClient _locationIqClient;
        private OpenWeatherMapClient _openWeatherMapClient;

    public ApiRequestTerminal(string configPath, string locationApiRules)
        {
        _locationIqClient = new LocationIqClient(configPath, locationApiRules);
        _openWeatherMapClient = new OpenWeatherMapClient(configPath);
        }

    public ApiResponse execute(string parameter, string location)
        {
            switch (parameter)
            {
                case "coordinates":
                    return _openWeatherMapClient.apiRequest(location);
                case "location":
                    // Use the name of a city like "New York", or city and country like "New York:USA"
                    string geolocation = _locationIqClient.apiRequest(location).value("geolocation");
                    return _openWeatherMapClient.apiRequest(geolocation);
                default:
                    throw new ArgumentException($"Invalid argument {parameter}.");
            }
        }
    }
}
