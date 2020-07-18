using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace HttpClientDemo011
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 添加到依赖注入的容器中
            //services.AddHttpClient();

            //services.AddHttpClient("github", c =>
            //{
            //    c.BaseAddress = new Uri("https://github.com/");
            //})
            //.AddHttpMessageHandler<MyHttpClientHandler>();
            //services.AddTransient<MyHttpClientHandler>();

            #region Polly - 重试、熔断、超时
            services.AddHttpClient("github", c =>
                {
                    c.BaseAddress = new Uri("https://github111.com/");
                })

                // 请求失败重试3次，每隔3秒重试一次
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(3)))

                // NuGet - Microsoft.Extensions.Http.Polly
                // 熔断 - 没有熔断，无效的地址会一直请求一直重试；熔断发现无效后再次请求会立刻返回错误不再重试
                // 连续出现2次，熔断10秒
                .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>().CircuitBreakerAsync(2, TimeSpan.FromSeconds(10), (ex, ts) =>
                {
                    // 状态一直熔断，则一直在10秒和2次之间循环
                    Console.WriteLine($"break here {ts.TotalMilliseconds}，异常：{ex.Exception.Message}");
                }, () =>
                {
                    // 恢复请求，之后恢复请求之后，才会继续执行失败重试-WaitAndRetryAsync
                    Console.WriteLine($"reset here ");
                }))
                // 超时
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(6));

            #endregion


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
