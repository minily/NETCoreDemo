using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MultiDI003.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IMyService _myService1;
        private readonly IMyService _myService2;

        private readonly IDataBase _dataBase;

        public WeatherForecastController(IEnumerable<IMyService> myServices, IDataBase dataBase,ILogger<WeatherForecastController> logger)
        {
            foreach (var service in myServices)
            {
                // switch case
                if (service is MyService1)
                {
                    _myService1 = service;
                }
                else if (service is MyService2)
                {
                    _myService2 = service;
                }
            }

            _dataBase = dataBase;

            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _myService1.Print();
            _myService2.Print();



            _dataBase.Print();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
