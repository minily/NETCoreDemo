using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HealthCheck010
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
            // 注入健康检查
            //services.AddHealthChecks();

            // 基于数据库是否连接健康
            //services.AddHealthChecks().AddMySql(Configuration.GetConnectionString("OilCenterDB"));

            // 数据库连接健康 - 可加过滤 (定义监测标签) - 安装NuGet包 - AspNetCore.HealthChecks.MySql
            //services.AddHealthChecks().AddMySql(Configuration.GetConnectionString("OilCenterDB"), tags: new[] { "mysql" });

            // 自定义健康检查（监控类 - 业务逻辑、内存、磁盘等等）
            services.AddHealthChecks()
                .AddMySql(Configuration.GetConnectionString("OilCenterDB"), tags: new[] { "mysql" })
                .AddMyHealthCheck();    // 自定义监控类注入到监控列表

            #region 设置为主动监控并推送健康状态
            // 主动推送应用运行健康状态
            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                // 延迟 N 秒推送
                options.Delay = TimeSpan.FromSeconds(5);
                // 每隔 N 秒推送
                options.Period = TimeSpan.FromSeconds(8);
            });

            // 将主动推送Publisher注入为单例模式
            services.AddSingleton<IHealthCheckPublisher, ReadinessPublisher>();
            #endregion

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 使用健康检查 （端口添加到项目属性，且可把端口配置到json文件方便维护）
            app.UseHealthChecks("/health", 8000, new HealthCheckOptions
            {
                ResponseWriter = WriteResponse
                //Predicate = (check) => check.Tags.Contains("mysql")
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 健康检查自定义响应格式
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static Task WriteResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }

    }
}
