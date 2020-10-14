using System;
using ApiClients;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.IO;
using DatabaseClient;

namespace WeatherAPI
{
    
    public class GeolocationWeatherApi : ControllerBase
    {
        static string configPath = Path.Combine(
                                   Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                   "Configs", "ApiConfigs.json");
        ApiRequestTerminal terminal = new ApiRequestTerminal(configPath);

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
