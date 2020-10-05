using System;
using ApiClients;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WeatherAPI
{
    
    public class GeolocationWeatherApi : ControllerBase
    {
        ApiRequestTerminal terminal = new ApiRequestTerminal();

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
