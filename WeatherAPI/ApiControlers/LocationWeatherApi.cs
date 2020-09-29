using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.ApiControlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationWeatherApi : ControllerBase
    {
        public string Get()
        {
        }
    }
}
