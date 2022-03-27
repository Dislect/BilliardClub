using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using BilliardClub.App_Data;
using BilliardClub.HangfireService;
using BilliardClub.Models;
using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BilliardClub
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddDbContext<Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure();
                    }));

            services.AddIdentity<User, IdentityRole>(options =>
                    {
                        options.Password.RequiredLength = 3;   // ����������� ����� ������
                        options.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
                        options.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
                        options.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
                        options.Password.RequireDigit = false; // ��������� �� �����
                    }
                )
                .AddEntityFrameworkStores<Context>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => Cart.GetCart(sp));
            services.AddTransient<CartService>();
            services.AddTransient<OrderService>();

            services.AddMvc(option => option.EnableEndpointRouting = false).AddNewtonsoftJson(options =>
                options.SerializerSettings.PreserveReferencesHandling  = Newtonsoft.Json.PreserveReferencesHandling.Objects
            );

            services.AddMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            // ��������� ��������� �� �������
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();

            // ���. ����������� ������
            app.UseStaticFiles();

            app.UseRouting();

            // ����������� �������������� � �����������
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard("/adm", new DashboardOptions()
            {
                Authorization = new[]
                {
                    new HangfireCustomBasicAuthenticationFilter{
                        User = "admin",
                        Pass = "admin"
                    }
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Main}/{id?}");
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
