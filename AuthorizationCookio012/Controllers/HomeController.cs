using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using AuthorizationCookio012.Models;

namespace AuthorizationCookio012.Controllers
{
    // 固定角色
    //[Authorize(Roles = "admin")]

    // 自定义策略
    [Authorize(Policy = "Permission")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("/login")]
        public IActionResult Login()
        {

            return View();
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (username == "minily" && password == "123456")
            {
                // 保存信息到 CookieAuthentication
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "Minily"));
                identity.AddClaim(new Claim(ClaimTypes.Sid, username));

                // 上下文签名添加上 CookieAuthentication
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return Redirect("/");
            }
            else
            {
                return BadRequest("username or password is error");
            }
        }

        [AllowAnonymous]
        [HttpGet("/denied")]
        public IActionResult Denied()
        {

            return View();
        }


        public IActionResult Index()
        {
            // 获取登录用户名 --- 获取Role同理
            var loginName = User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid).Value;

            ViewBag.Name = User.Identity.Name;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
