using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Configuration006.Controllers
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
        private readonly AppSetting _appSetting;

        /// <summary>
        /// AppSetting appSetting
        /// </summary>
        /// <param name="appSetting"></param>
        /// <param name="logger"></param>
        public WeatherForecastController(IOptionsSnapshot<AppSetting> option, ILogger<WeatherForecastController> logger)
        {
            _appSetting = option.Value;
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            var content = $"名字：{_appSetting.Name }，年龄：{_appSetting.Age}";
            Console.WriteLine(content);

            var rng = new Random();
            return content;
        }
    }
}
