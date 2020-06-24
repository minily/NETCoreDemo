using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationCookio012.Permission
{
    /// <summary>
    /// 自定义策略 - 验证类
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var url = string.Empty;
            var method = string.Empty;

            // 用户认证阶段，.NetCore 3.0是EndPoint
            if (context.Resource is RouteEndpoint)
            {
                var route = context.Resource as RouteEndpoint;
                if (route.RoutePattern.PathSegments.Count > 0)
                {
                    url = $"{route.RoutePattern.Defaults["controller"]?.ToString()?.ToLower()} / {route.RoutePattern.Defaults["action"]?.ToString()?.ToLower()}";
                }
                else
                {
                    url = route.RoutePattern.RawText?.ToLower();
                }
            }
            else
            {
                var filter = context.Resource as AuthorizationFilterContext;
                url = filter?.HttpContext?.Request?.Path.Value?.ToString();
                method = filter?.HttpContext?.Request?.Method;
            }

            var userPermissions = requirement.UserPermissions;
            var isAuthenticated = context?.User?.Identity?.IsAuthenticated;
            if (isAuthenticated.HasValue && isAuthenticated.Value)
            {
                // 判断访问的页面是否需要权限验证
                var count = userPermissions.GroupBy(g => g.Url).Where(w => w.Key.ToLower() == url).Count();
                if (count > 0)
                {
                    // 判断当前登录用户是否在我们验证的权限集合里
                    var userName = context.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid)?.Value;
                    var isExist = userPermissions.Where(p => p.UserName == userName && p.Url == url).Count() > 0;
                    if (isExist)
                    {
                        context.Succeed(requirement);
                    }
                }
                else
                {
                    // 不需要验证的页面返回成功，亦可返回失败
                    context.Succeed(requirement);
                }
            }
            else {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
