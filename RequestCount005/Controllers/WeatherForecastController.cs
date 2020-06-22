using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace RequestCount005.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IRequestCountService _requestCountService;

        public WeatherForecastController(IRequestCountService requestCountService)
        {
            _requestCountService = requestCountService;
        }

        [HttpGet("/count")]
        public string GetRequestCount()
        {
            var requestCountDic = _requestCountService.RequestList;
            var count = requestCountDic.Count;

            return $"总请求数：{count}，当前请求数：{requestCountDic.Count(c => c.Value)}";
        }

        [HttpGet("/ok")]
        public IActionResult GetOK()
        {
            Thread.Sleep(10000);
            var jsonOk = new JsonResult("ok");
            return jsonOk;
        }
    }
}
