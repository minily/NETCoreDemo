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
            // ��ӵ�����ע���������
            //services.AddHttpClient();

            //services.AddHttpClient("github", c =>
            //{
            //    c.BaseAddress = new Uri("https://github.com/");
            //})
            //.AddHttpMessageHandler<MyHttpClientHandler>();
            //services.AddTransient<MyHttpClientHandler>();

            #region Polly - ���ԡ��۶ϡ���ʱ
            services.AddHttpClient("github", c =>
                {
                    c.BaseAddress = new Uri("https://github111.com/");
                })

                // ����ʧ������3�Σ�ÿ��3������һ��
                .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(3)))

                // NuGet - Microsoft.Extensions.Http.Polly
                // �۶� - û���۶ϣ���Ч�ĵ�ַ��һֱ����һֱ���ԣ��۶Ϸ�����Ч���ٴ���������̷��ش���������
                // ��������2�Σ��۶�10��
                .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<Exception>().CircuitBreakerAsync(2, TimeSpan.FromSeconds(10), (ex, ts) =>
                {
                    // ״̬һֱ�۶ϣ���һֱ��10���2��֮��ѭ��
                    Console.WriteLine($"break here {ts.TotalMilliseconds}���쳣��{ex.Exception.Message}");
                }, () =>
                {
                    // �ָ�����֮��ָ�����֮�󣬲Ż����ִ��ʧ������-WaitAndRetryAsync
                    Console.WriteLine($"reset here ");
                }))
                // ��ʱ
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
