using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationCookio012.Permission;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthorizationCookio012
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
            // 固定角色
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = new PathString("/login");
            //        options.AccessDeniedPath = new PathString("/denied");
            //    });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Permission", policy =>
                {
                    var userPermissions = new List<UserPermission> {
                        new UserPermission{ Url = "/home/index", UserName="minily"},
                        //new UserPermission{ Url = "/", UserName="minily"},
                        new UserPermission{ Url = "/home/privacy", UserName="admin"}
                    };
                    policy.Requirements.Add(new PermissionRequirement("/denied", userPermissions));
                });
            })
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
            {
                options.LoginPath = new PathString("/login");
                options.AccessDeniedPath = new PathString("/denied");
            });

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();



            services.AddControllersWithViews(opt =>
            {
                opt.EnableEndpointRouting = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // 认证
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseRouting();

            // 授权
            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseMvcWithDefaultRoute()
                .UsePathBase("/{controller=Home}/{action=Index}/{id?}");
        }
    }
}
