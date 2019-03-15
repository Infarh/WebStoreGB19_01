using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public static class Log4NetExtensions
    {
        public static ILoggerFactory AddLog4Net(
            this ILoggerFactory factory,
            string configurationFile = "log4net.config")
        {
            var file = new FileInfo(configurationFile);
            if (!Path.IsPathRooted(configurationFile))
            {
                var assembly = Assembly.GetEntryAssembly();
                var dir = Path.GetDirectoryName(assembly.Location);
                
                Debug.Assert(dir != null, nameof(dir) + " != null");
                configurationFile = Path.Combine(dir, configurationFile);
            }

            factory.AddProvider(new Log4NetLoggerProvider(configurationFile));
            return factory;
        }
    }
}
