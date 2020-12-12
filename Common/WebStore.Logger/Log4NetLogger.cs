using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebStore.Logger
{
    public class Log4NetLogger
	{
        private readonly ILog _log;

        public Log4NetLogger(string categoryName, XmlElement configuration)
        {
            var logger_repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));
            _log = LogManager.GetLogger(logger_repository.Name, categoryName);
            log4net.Config.XmlConfigurator.Configure(logger_repository, configuration);
        }

		public IDisposable BeginScope<TState>(TState state)
		{
			return null;
		}

		public bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                default: throw new ArgumentOutOfRangeException(nameof(level), level, null);
                case LogLevel.None: return false;
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _log.IsDebugEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId Id, TState state, Exception error, Func<TState, Exception, string> formatter)
        {
            if (formatter is null) throw new ArgumentNullException(nameof(formatter));
            if (!IsEnabled(logLevel)) return;
            var log_message = formatter(state, error);
            if (string.IsNullOrEmpty(log_message) && error is null) return;
            switch (logLevel)
            {
                default: throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                case LogLevel.None: break;

                case LogLevel.Trace:
                case LogLevel.Debug:
                    _log.Debug(log_message);
                    break;
                case LogLevel.Information:
                    _log.Info(log_message);
                    break;
                case LogLevel.Warning:
                    _log.Warn(log_message);
                    break;
                case LogLevel.Error:
                    _log.Error(log_message, error);
                    break;
                case LogLevel.Critical:
                    _log.Fatal(log_message, error);
                    break;
            }
        }
    }
}
