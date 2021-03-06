using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
namespace WebStore.ServiceHosting
{
	public class Program
	{
		public static IConfigurationRoot _builtConfig;

		public static void Main(string[] args)
		{
			_builtConfig = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.AddCommandLine(args)
			.Build();
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.Debug()
				//.WriteTo.RollingFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Logs/ServiceHosting[{DateTime.Now:yyyy-M-ddTHH}].log"))
				.WriteTo.File(new JsonFormatter(",", true), (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"Logs/ServiceHosting[{DateTime.Now:yyyy-MM-ddTHH}].log.json")))
				.CreateLogger()
				;
			return
			Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>()
				.UseKestrel()
				.UseUrls(_builtConfig["hosturl"])
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.ClearProviders();
					logging.AddSerilog();
				});
			});
		}
	}
}
