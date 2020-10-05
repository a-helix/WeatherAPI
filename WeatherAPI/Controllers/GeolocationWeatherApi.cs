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
        [Route("/coordinates/weather/{location}")]
        public ActionResult<string> getLocation(string location)
        {
            try
            {
                ApiResponse response = terminal.execute("coordinates", location);
                return Ok(response.json());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
