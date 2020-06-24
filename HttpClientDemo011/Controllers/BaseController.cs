using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDemo011.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("/get")]
        public async Task<string> GetContent()
        {
            // 创建一个HttpClient
            //var httpClient = _httpClientFactory.CreateClient();
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://www.github.com/");

            var httpClient = _httpClientFactory.CreateClient("github");
            
            var request = new HttpRequestMessage(HttpMethod.Get, "/settings");


            // 发送情况并判断是否请求成功
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }

            return "request error";
        }

    }
}
