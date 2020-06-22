using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MW004
{
    /// <summary>
    /// Middleware 中间件
    /// </summary>
    public class MyMW
    {
        private readonly RequestDelegate _next;

        public MyMW(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 进行访问权限验证，请求先到这里，在这里权限验证通过之后，再转到Controller，然后再返回响应
            // context.Request.Query

            // 不过校验直接返回context
            //await _next(context);

            // 如果验证不通过直接在这里返回
            await context.Response.WriteAsync("user error");
        }
    }

    public static class MyMWExtensions
    {
        public static IApplicationBuilder UseMyMW(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMW>();
        }
    }
}
