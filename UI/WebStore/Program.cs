using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace WebStore
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
			//CreateHostBuilder(args).Build().RunAsync();
		}

        public static IHostBuilder CreateHostBuilder(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.Debug()
				.WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/WebStoreLog.txt"), rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
				.CreateLogger();
			return
			Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>()
				.UseKestrel()
				.UseUrls(_builtConfig["hosturl"])
				//.UseHttpSys(opt => opt.MaxAccepts = 5)
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
