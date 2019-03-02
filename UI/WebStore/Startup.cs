using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Clients.Employees;
using WebStore.Clients.Orders;
using WebStore.Clients.Products;
using WebStore.Clients.Values;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Api;
using WebStore.Interfaces.Services;
using WebStore.Services;
using WebStore.Services.Sql;

namespace WebStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config) => Configuration = config;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTransient<IValuesService, ValuesClient>();
            services.AddTransient<IEmployeesData, EmployeesClient>();
            //services.AddScoped<IEmployeesData, InMemoryEmployeesData>();
            //services.AddScoped<IProductData, InMemoryProductData>();
            services.AddScoped<IProductData, ProductsClient>();
            //services.AddScoped<IProductData, SqlProductData>();
            services.AddScoped<ICartService, CookieCartService>();
            services.AddScoped<IOrderService, OrdersClient>();
            //services.AddScoped<IOrderService, SqlOrderService>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<WebStoreContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.AllowedForNewUsers = true;

                //opt.User.RequireUniqueEmail = true; // hack!!!
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.HttpOnly = true;
                opt.Cookie.Expiration = TimeSpan.FromDays(150);

                opt.LoginPath = "/Account/Login";
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
                //route.MapRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                route.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
