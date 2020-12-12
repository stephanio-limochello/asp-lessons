using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Services.Utils
{
	public static partial class Extensions
	{
		public static void LogException(this Exception exception, ILogger logger)
		{
			logger.LogError(exception, exception.Message);
		}

		public static void LogInfo(this string message, ILogger logger)
		{
			logger.LogInformation(message);
		}
	}
}
