using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestCount005
{
    /// <summary>
    /// 权限校验中间件
    /// </summary>
    public class RequestCountMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCountMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IRequestCountService requestCountService)
        {
            // 获取当前请求的标识
            var identify = context.TraceIdentifier;
            requestCountService.RequestList.Add(identify, true);
            await _next(context);
            requestCountService.RequestList[identify] = false;
        }
    }

    /// <summary>
    /// 中间件扩展引用
    /// </summary>
    public static class RequiestCountMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCountMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCountMiddleware>();
        }
    }
}
