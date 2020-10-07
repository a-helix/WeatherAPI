using System;
using ApiClients;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.IO;

namespace WeatherAPI
{
    
    public class GeolocationWeatherApi : ControllerBase
    {
        static string configPath = Path.Combine(
                                   Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                   "Configs", "ApiClientKeys.json");
        ApiRequestTerminal terminal = new ApiRequestTerminal(configPath);

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("/weather/coordinates/{coordinates}")]
        public ActionResult<string> getCoordinates(string coordinates)
        {
            try
            {
                ApiResponse response = terminal.execute("coordinates", coordinates);
                return Ok(response.json());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
