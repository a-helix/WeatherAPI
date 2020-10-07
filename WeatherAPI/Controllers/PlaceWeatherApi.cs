using System;
using ApiClients;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp.Extensions;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;

namespace WeatherAPI
{

    [ApiController]
    
    public class PlaceWeatherApi : ControllerBase
    {
        static string configPath = Path.Combine(
                                   Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                   "Configs", "ApiClientKeys.json");
        ApiRequestTerminal terminal = new ApiRequestTerminal(configPath);
        private ILogger<PlaceWeatherApi> _logger;

        public PlaceWeatherApi(ILogger<PlaceWeatherApi> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("/weather/place/{place}")]
        public ActionResult<string> getWeather(string place)
        {
            try
            {
                ApiResponse response = terminal.execute("location", place);
                _logger.LogInformation($"Successful request: {place}");
                return Ok(response.json());
            }
            catch(Exception e)
            {
                _logger.LogError($"EXCEPTION: {e}");
                return BadRequest($"Invalid parameter: {place}");
            }
        }
    }
}
