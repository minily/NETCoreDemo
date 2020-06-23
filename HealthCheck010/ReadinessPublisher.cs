using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck010
{
    /// <summary>
    /// 健康检查发布器 - 主动推送应用健康
    /// </summary>
    public class ReadinessPublisher : IHealthCheckPublisher
    {
        private readonly ILogger _logger;

        public ReadinessPublisher(ILogger<ReadinessPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            // 报告的状态、报告持续时长、报告记录次数
            _logger.LogInformation($"{DateTime.Now} 准备探针状态：{report.Status},{report.TotalDuration}，{report.Entries.Count}");

            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }
    }
}
