using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClients
{
    public class ApiRequestTerminal
    {
        LocationIqClient locationIqClient;
        OpenWeatherMapClient openWeatherMapClient;

    public ApiRequestTerminal()
        {
            locationIqClient = new LocationIqClient();
            openWeatherMapClient = new OpenWeatherMapClient();
        }

    public ApiResponse execute(string parameter, string location)
        {
            switch (parameter)
            {
                case "coordinates":
                    return openWeatherMapClient.apiRequest(location);
                case "location":
                    string geolocation = locationIqClient.apiRequest(location).value("geolocation");
                    return openWeatherMapClient.apiRequest(geolocation);
                default:
                    throw new ArgumentException($"Invalid argument {0}.", parameter);
            }
        }
    }
}
