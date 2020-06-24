using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthenticationToken013.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthenticationToken013.Controllers
{
    [Authorize(Policy = "Permission")]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly PermissionRequirement _permissionRequirement;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, PermissionRequirement permissionRequirement)
        {
            _permissionRequirement = permissionRequirement;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public IActionResult Login(string username, string password)
        {
            if (username == "minily" && password == "123456")
            {
                var claims = new Claim[] {
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Name, "Minily"),
                new Claim(ClaimTypes.Sid, username),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_permissionRequirement.Expiration.TotalSeconds).ToString())
                };
                var token = JwtToken.BuildJwtToken(claims, _permissionRequirement);
                return new JsonResult(token);
            }
            else
            {
                return BadRequest("username or password is error");
            }
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            var list = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToList()
            ;


            list.Add(new WeatherForecast { TemperatureC = 10, Summary = User.Identity.Name, Date = DateTime.Now });
            return list.ToArray();
        }
    }
}
