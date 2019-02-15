using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Infrastucture.Conventions;
using WebStore.Infrastucture.Filters;
using WebStore.Infrastucture.Implementations;
using WebStore.Infrastucture.Interfaces;
using WebStore.Infrastucture.Middleware;

namespace WebStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config) => Configuration = config;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<IEmployeesData, InMemoryEmployeesData>();
            //services.AddScoped<IProductData, InMemoryProductData>();
            services.AddScoped<IProductData, SqlProductData>();

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireDigit = true;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.AllowedForNewUsers = true;

                //opt.User.RequireUniqueEmail = true; // hack!!!
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.HttpOnly = true;
                opt.Cookie.Expiration = TimeSpan.FromDays(150);

                opt.LoginPath = "/Accout/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenided";

                opt.SlidingExpiration = true;
            });

            services.AddDbContext<WebStoreContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(route =>
            {
                route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
