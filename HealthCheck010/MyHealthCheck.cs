using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck010
{
    /// <summary>
    /// 应用健康监控器 - 需要主动拉取（请求）
    /// 继承IHealthCheck接口
    /// </summary>
    public class MyHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // 处理业务逻辑，来判断应用是否健康，以及返回的 HealthCheckResult 状态

            //return Task.FromResult(HealthCheckResult.Degraded("亚健康"));

            return Task.FromResult(HealthCheckResult.Unhealthy("不健康"));
        }
    }

    /// <summary>
    /// 创建自定义健康检查扩展类
    /// </summary>
    public static class MyHealthCheckExtensions
    {
        /// <summary>
        /// 封装.net core调用风格
        /// </summary>
        /// <param name="builder">扩展方法this</param>
        /// <returns></returns>
        public static IHealthChecksBuilder AddMyHealthCheck(this IHealthChecksBuilder builder)
        {
            builder.AddCheck<MyHealthCheck>("MyHealthCheck", HealthStatus.Healthy);
            return builder;
        }

    }
}
