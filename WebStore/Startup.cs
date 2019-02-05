using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Infrastucture.Conventions;
using WebStore.Infrastucture.Filters;
using WebStore.Infrastucture.Implementations;
using WebStore.Infrastucture.Interfaces;
using WebStore.Infrastucture.Middleware;

namespace WebStore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                //options.Filters.Add<TestActionFilter>();
                //options.Filters.Add(typeof(TestResultFilter));
                //options.Filters.Add(new TestActionFilter());

                //options.Conventions.Add(new TestConvention());
            });

            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            //services.AddScoped<IEmployeesData, InMemoryEmployeesData>();
            //services.AddTransient<IEmployeesData, InMemoryEmployeesData>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            //app.Use(async (context, next) =>
            //{
            //    if (true)
            //    {
            //        await context.Response.WriteAsync("Некий ответ");
            //    }
            //    else
            //        await next();
            //});

            app.MapWhen(
                ctx => ctx.Request.Query.ContainsKey("TestId")
                       && ctx.Request.Query["TestId"] == "5",
                a =>
                {
                    a.Run(async ctx => await ctx.Response.WriteAsync("Test data id 5"));
                }
            );

            app.Map("/TestPath", a =>
            {
                a.Run(async ctx => await ctx.Response.WriteAsync("Test data"));
            });

            //app.UseMiddleware<TestMiddleware>();
            app.UseTestMiddleware();

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(route =>
            {
                route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            //app.Run(ctx => ctx.Response.WriteAsync("Hello world"));
        }
    }
}
