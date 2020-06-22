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
                logger.Info("����NLog008��Ŀ");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                // �ش������Ե��쳣ʹ�� Fatal()
                logger.Fatal(ex, $"����Nlog008��Ŀ�쳣��{ex.Message}");
            }
            finally
            {
                // �ͷ�NLog
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })

                // ��־ʹ�÷�ʽ����һ���ģ���ʹController������ע�����ILogger
                // ��.NET Core����ʹ�����Use����־���ͣ���չ��װ�ķǳ��ã�����һ����־���ͣ���Ŀ�����ط�����Ҫά��
                .UseNLog();
    }
}
