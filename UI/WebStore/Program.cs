using System.Reflection;
using System.Xml;
using log4net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var log4NetConfigXml = new XmlDocument();
            const string configFileName = "log4net.config";
            
            log4NetConfigXml.Load(configFileName);
            
            var repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repository, log4NetConfigXml["log4net"]);

            var log = log4net.LogManager.GetLogger(typeof(Program));
            
            log.Info("Starting");
            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
