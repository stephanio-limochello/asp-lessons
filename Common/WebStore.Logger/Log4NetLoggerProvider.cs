using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebStore.Logger
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly string _configurationFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>();

        public Log4NetLoggerProvider(string ConfigurationFile)
		{
            _configurationFile = ConfigurationFile;
        }

        public ILogger CreateLogger(string CategoryName)
		{
            return (ILogger)_loggers.GetOrAdd(CategoryName, category =>
            {
                var xml = new XmlDocument();
                xml.Load(_configurationFile);
                return new Log4NetLogger(category, xml["log4net"]);
            });
        }

        public void Dispose()
		{
            _loggers.Clear();
        }
    }
}
