using System;
using ApiClients;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp.Extensions;

namespace WeatherAPI
{

    [ApiController]
    
    public class PlaceWeatherApi : ControllerBase
    {
        ApiRequestTerminal terminal = new ApiRequestTerminal();

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Route("/place/weather/{place}")]
        public ActionResult<string> getWeather([FromRoute]string place)
        {
            try
            {
                ApiResponse response = terminal.execute("location", place);
                return Ok(response.json());
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
