using System;
using ApiClients;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.IO;
using DatabaseClients;
using Credentials;

namespace WeatherAPI
{
    
    public class GeolocationWeatherApi : ControllerBase
    {
        static string configPath = Path.Combine(
                                   Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                   "Configs", "ApiConfigs.json");
        static JsonFileContent configContent = new JsonFileContent(configPath);
        static string  databaseUrl = (string) configContent.Value("databaseUrl");
        static MongoDatabaseClient client = new MongoDatabaseClient(databaseUrl, "Areas", "areas");
        ApiRequestTerminal terminal = new ApiRequestTerminal(configPath, client);

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("/weather/coordinates/{coordinates}")]
        public ActionResult<string> getCoordinates(string coordinates)
        {
            try
            {
                /// <summary>Use the name of a city like "New York", or city and country like "New York:USA"</summary>
                ApiResponse response = terminal.Execute("coordinates", coordinates);
                return Ok(response.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
