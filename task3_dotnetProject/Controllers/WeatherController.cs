using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using task3_dotnetProject.JSONmodels.JSONdatatypes;
using task3_dotnetProject.Services;

namespace task3_dotnetProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly OWMforecastService OWMservice;


        public WeatherController(OWMforecastService OWMservice)
        {
            this.OWMservice = OWMservice;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetAsync(string city, string date)
        {
            JSONcomponent jsonObj = await this.OWMservice.GetJSONWeatherForecasts(city);

            return Ok(jsonObj.Print(0));
        }
    }
}
