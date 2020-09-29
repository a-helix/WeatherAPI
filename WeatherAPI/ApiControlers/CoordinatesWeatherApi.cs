using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ApiClients;
using RabbitChat;

namespace WeatherAPI.ApiControlers
{
    [ApiController]
    [Route("[controller]"]
    public class CoordinatesWeatherApi : ControllerBase
    {
        [HttpGet("lat={lat}&lon={lon}")]
        public string Get(string lat, string lon)
        {
            private readonly ILogger<CoordinatesWeatherApi> _logger;
            OpenWeatherMapClient client = new OpenWeatherMapClient();
            Consumer consumer = new Consumer("localhoste", "CoordinatesWeatherApi", "CoordinatesWeatherApi");
            string response = null;
            string coordinates = String.Join(';', lat, lon);
            //check db
            try
            {
            client.apiRequest(coordinates);
            response = await consumer.receive(coordinates);
            _logger.LogInformation("Successful request. Coordinates: {1}", e, coordinates);
            return response;
            }
            catch(Exception e)
            {
            _logger.LogInformation("Exception: {0} Coordinates: {1}", e, coordinates);
            return response;
            }           
        }
    }
}
