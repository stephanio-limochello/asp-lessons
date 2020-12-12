using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

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
		}

        public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return
			Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>()
				.UseKestrel()
				.UseUrls(_builtConfig["hosturl"])
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
				.UseSerilog((host, log) => log.ReadFrom.Configuration(host.Configuration)
						   .MinimumLevel.Debug()
						   .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
						   .Enrich.FromLogContext()
						   .WriteTo.Console(
								outputTemplate: "[{Timestamp:HH:mm:ss.fff} {Level:u3}]{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}")
						   .WriteTo.RollingFile($@".\Log\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.txt")
						   .WriteTo.File(new JsonFormatter(",", true), $@".\Log\WebStore[{DateTime.Now:yyyy-MM-ddTHH-mm-ss}].log.json")
						   .WriteTo.Seq("http://localhost:5341/"));
			});
		}
    }
}
