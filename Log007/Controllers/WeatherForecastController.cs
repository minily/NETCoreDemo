using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Log007.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            //LogLevel
            _logger.LogTrace("   LogTrace");
            _logger.LogDebug("   LogDebug");
            _logger.LogInformation("   LogInformation");
            _logger.LogWarning("   LogWarning");
            _logger.LogError("   LogError");
            _logger.LogCritical("   LogCritical");

            return "Log output complete";
        }
    }
}
