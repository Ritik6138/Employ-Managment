using Employee.Repository.Interfaces;
using Employee.Repository.Models;
using Employee.Repository.Repositories;
using Employee.Service.Interfaces;
using Employee.Service.Services;
using EmployeeManagement.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace EmployeeManagement
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;

        }
                
        public void ConfigureServices(IServiceCollection services)
        {                  
            services.AddMvc().AddXmlDataContractSerializerFormatters();

            services.AddSingleton<IAuthRepository, InMemoryAuthRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<IEmployeeRepository, InMemoryEmployeeRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<BreadCrumbService>();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });
            services.AddHttpContextAccessor();
            services.AddDataProtection()
             .PersistKeysToFileSystem(new DirectoryInfo(Path.GetTempPath()));
            services.AddControllersWithViews().AddXmlSerializerFormatters(); ;
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login"; // Redirect to login page
            });
        }
        public void Configure(IApplicationBuilder app,
                       IWebHostEnvironment env,
                       ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();  

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Auth}/{action=Login}/{id?}"); // Default to Login
            });
        }

    }
}
