using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Fluent;
using NLog.Web;

namespace NLog008
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("启动NLog008项目");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                // 重大灾难性的异常使用 Fatal()
                logger.Fatal(ex, $"启动Nlog008项目异常：{ex.Message}");
            }
            finally
            {
                // 释放NLog
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })

                // 日志使用方式都是一样的，即使Controller里以来注入的是ILogger
                // 但.NET Core都会使用最后Use的日志类型，扩展封装的非常好，更换一种日志类型，项目其他地方不需要维护
                .UseNLog();
    }
}
