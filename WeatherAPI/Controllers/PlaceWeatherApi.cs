using System;
using ApiClients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using DatabaseClients;
using Credentials;

namespace WeatherAPI
{

    [ApiController]
    
    public class PlaceWeatherApi : ControllerBase
    {
        //api configurations
        static string apiConfigPath = Path.Combine(
                                   Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                   "Configs", "ApiConfigs.json");
        static JsonFileContent configContent = new JsonFileContent(apiConfigPath);
        static string databaseUrl = (string)configContent.Parameter("databaseUrl");
        static MongoDatabaseClient client = new MongoDatabaseClient(databaseUrl, "Areas", "areas");
        ApiRequestTerminal terminal = new ApiRequestTerminal(apiConfigPath, client);
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
                ApiResponse response = terminal.Execute("location", place);
                _logger.LogInformation($"Successful request: {place}");
                return Ok(response.ToString());
            }
            catch(Exception e)
            {
                _logger.LogError($"EXCEPTION: {e}");
                return BadRequest($"Invalid parameter: {place}");
            }
        }
    }
}
