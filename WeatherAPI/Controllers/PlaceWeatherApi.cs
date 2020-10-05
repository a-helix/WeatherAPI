using System;
using ApiClients;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp.Extensions;
using Microsoft.Extensions.Logging;

namespace WeatherAPI
{

    [ApiController]
    
    public class PlaceWeatherApi : ControllerBase
    {
        ApiRequestTerminal terminal = new ApiRequestTerminal();
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
                return BadRequest(e);
            }
        }
    }
}
