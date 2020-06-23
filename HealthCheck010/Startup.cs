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
            // ע�뽡�����
            //services.AddHealthChecks();

            // �������ݿ��Ƿ����ӽ���
            //services.AddHealthChecks().AddMySql(Configuration.GetConnectionString("OilCenterDB"));

            // ���ݿ����ӽ��� - �ɼӹ��� (�������ǩ) - ��װNuGet�� - AspNetCore.HealthChecks.MySql
            //services.AddHealthChecks().AddMySql(Configuration.GetConnectionString("OilCenterDB"), tags: new[] { "mysql" });

            // �Զ��彡����飨����� - ҵ���߼����ڴ桢���̵ȵȣ�
            services.AddHealthChecks()
                .AddMySql(Configuration.GetConnectionString("OilCenterDB"), tags: new[] { "mysql" })
                .AddMyHealthCheck();    // �Զ�������ע�뵽����б�

            #region ����Ϊ������ز����ͽ���״̬
            // ��������Ӧ�����н���״̬
            services.Configure<HealthCheckPublisherOptions>(options =>
            {
                // �ӳ� N ������
                options.Delay = TimeSpan.FromSeconds(5);
                // ÿ�� N ������
                options.Period = TimeSpan.FromSeconds(8);
            });

            // ����������Publisherע��Ϊ����ģʽ
            services.AddSingleton<IHealthCheckPublisher, ReadinessPublisher>();
            #endregion

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ʹ�ý������ ���˿���ӵ���Ŀ���ԣ��ҿɰѶ˿����õ�json�ļ�����ά����
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
        /// ��������Զ�����Ӧ��ʽ
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
