using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilliardClub.App_Data;
using BilliardClub.Models;
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
            services.AddDbContext<Context>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
                    {
                        options.Password.RequiredLength = 3;   // минимальная длина пароля
                        options.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
                        options.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
                        options.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
                        options.Password.RequireDigit = false; // требуются ли цифры
                    }
                )
                .AddEntityFrameworkStores<Context>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => Cart.GetCart(sp));
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            // подробные сообщения об ошибках
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();

            // исп. статических файлов
            app.UseStaticFiles();

            app.UseRouting();

            // подключение аутентификации и авторизации
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Main}/{id?}");
            });
        }
    }
}
