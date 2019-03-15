using System.Collections.Concurrent;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly string _ConfigurationFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new ConcurrentDictionary<string, Log4NetLogger>();

        public Log4NetLoggerProvider(string configurationFile) => _ConfigurationFile = configurationFile;

        public ILogger CreateLogger(string categoryName)
        {
            return _Loggers.GetOrAdd(categoryName, category =>
            {
                var xml = new XmlDocument();
                var file_name = _ConfigurationFile;
                xml.Load(file_name);
                return new Log4NetLogger(category, xml["log4net"]);
            });
        }

        public void Dispose()
        {
            _Loggers.Clear();
        }
    }
}
