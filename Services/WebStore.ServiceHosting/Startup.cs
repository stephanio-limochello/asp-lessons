using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Services.Data;
using WebStore.Services.Interfaces;
using WebStore.Services.Products.InCookies;
using WebStore.Services.Products.InSQL;

namespace WebStore.ServiceHosting
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public IConfiguration _configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<WebStoreDB>(opt =>
				opt.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
					optionBuilder =>
						optionBuilder.MigrationsAssembly("WebStore.DAL")));
			services.AddTransient<WebStoreDBInitializer>();

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebStore.WebAPI", Version = "v1" });

				const string webstore_api_xml = "WebStore.ServiceHosting.xml";
				const string webstore_domain_xml = "WebStore.Domain.xml";
				string debug_path = Directory.GetCurrentDirectory();
#if DEBUG
				debug_path = "bin/Debug/net5.0";
#else
				//if (debug_path.Contains("WebStore.ServiceHosting")) debug_path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..");
#endif
				c.IncludeXmlComments(webstore_api_xml);
				if (File.Exists(webstore_domain_xml))
					c.IncludeXmlComments(webstore_domain_xml);
				else if (File.Exists(Path.Combine(debug_path, webstore_domain_xml)))
					c.IncludeXmlComments(Path.Combine(debug_path, webstore_domain_xml));
			});
			services.AddIdentity<User, Role>(opt => { })
			.AddEntityFrameworkStores<WebStoreDB>()
			.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(opt =>
			{
#if DEBUG
				opt.Password.RequiredLength = 3;
				opt.Password.RequireDigit = false;
				opt.Password.RequireLowercase = false;
				opt.Password.RequireUppercase = false;
				opt.Password.RequireNonAlphanumeric = false;
				opt.Password.RequiredUniqueChars = 3;

#endif
				opt.User.RequireUniqueEmail = false;
				opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

				opt.Lockout.AllowedForNewUsers = true;
				opt.Lockout.MaxFailedAccessAttempts = 10;
				opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
			});

			services.AddScoped<IProductData, SqlProductData>();
			services.AddScoped<ICartService, CookiesCartService>();
			services.AddScoped<IOrderService, SqlOrderService>();
			services.AddScoped<IEmployeesData, SqlEmployeesData>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebStoreDBInitializer db)
		{
			db.Initialize();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebStore.ServiceHosting v1"));
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=swagger}/{action=Index}/{id?}"
				);
			});
		}
	}
}
