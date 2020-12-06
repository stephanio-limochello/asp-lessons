using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Logger
{
    public static class Log4NetExtensions
    {
        public static ILoggerFactory AddLog4Net(this ILoggerFactory Factory, string ConfigurationFile = "log4net.config")
        {
            if (!Path.IsPathRooted(ConfigurationFile))
            {
                var assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Could not identify assembly with application entry point"); ;
                var dir = Path.GetDirectoryName(assembly.Location) ?? throw new InvalidOperationException("Could not determine location path of assembly with application entry point"); ;
                ConfigurationFile = Path.Combine(dir, ConfigurationFile);
            }
            Factory.AddProvider(new Log4NetLoggerProvider(ConfigurationFile));
            return Factory;
        }
    }
}
