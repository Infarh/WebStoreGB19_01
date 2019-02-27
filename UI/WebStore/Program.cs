using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Services.Data;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var db = services.GetRequiredService<WebStoreContext>();
                    db.Initialize();
                    services.InitializeIdentityAsync().Wait();
                }
                catch (Exception e)
                {
                    services.GetRequiredService<ILogger<Program>>()
                        .LogError(e, "Ошибка инициализации контекста БД");    
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
