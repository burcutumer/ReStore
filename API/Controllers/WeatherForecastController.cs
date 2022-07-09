using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly BurcuServis _burcuServis;
        public WeatherForecastController(BurcuServis servis)
        {
            _burcuServis = servis;
        }

        [HttpGet]
        public int Get()
        {
            var x = _burcuServis.Age;
            _burcuServis.Age = 110;
            return x;
        }
    }
}
