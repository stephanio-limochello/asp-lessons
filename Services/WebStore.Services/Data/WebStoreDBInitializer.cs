using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
		private readonly ILogger<WebStoreDBInitializer> _Logger;

		public WebStoreDBInitializer(WebStoreDB db, UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<WebStoreDBInitializer> logger)
        {
            _db = db;
            _UserManager = userManager;
            _RoleManager = roleManager;
			_Logger = logger;
		}

        public void Initialize()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Database initialization...");


            var db = _db.Database;
            if (db.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("Database migration...");
                db.Migrate();
                _Logger.LogInformation("Database migration completed successfully {0}ms", timer.ElapsedMilliseconds);
            }
            else
                _Logger.LogInformation("No database migration required");

            InitializeProducts();
            _Logger.LogInformation("Products are initialized {0} ms", timer.ElapsedMilliseconds);
            InitializeEmployees();
            InitializeIdentityAsync().Wait();
            _Logger.LogInformation("Identity initialization finished {0:0.###}s", timer.Elapsed.TotalSeconds);
            _Logger.LogInformation("Database initialization completed successfully {0:0.###}s", timer.Elapsed.TotalSeconds);
        }

        private void InitializeProducts()
        {
            var timer = Stopwatch.StartNew();
            _Logger.LogInformation("Product catalog initialization...");
            if (_db.Products.Any())
            {
                _Logger.LogInformation("Product catalog initialization is not required");
                return;
            }
            var db = _db.Database;

            using (db.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductSections] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductSections] OFF");

                db.CommitTransaction();
            }
            _Logger.LogInformation("Categories are initialized {0} ms", timer.ElapsedMilliseconds);

            using (db.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductBrands] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[ProductBrands] OFF");

                db.CommitTransaction();
            }
            _Logger.LogInformation("Brands are initialized {0}ms", timer.ElapsedMilliseconds);

            using (db.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                db.CommitTransaction();
            }
            _Logger.LogInformation("Items have been initialized {0}ms", timer.ElapsedMilliseconds);
        }

        private void InitializeEmployees()
        {
            if (_db.Employees.Any()) return;
            using (_db.Database.BeginTransaction())
            {
                TestData.Employees.ForEach(employee => employee.Id = 0);
                _db.Employees.AddRange(TestData.Employees);
                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
        }

        private async Task InitializeIdentityAsync()
        {
            _Logger.LogInformation("Identity initialization...");
            var timer = Stopwatch.StartNew();
            async Task CheckRoleExist(string RoleName)
            {
                if (!await _RoleManager.RoleExistsAsync(RoleName))
                {
                    _Logger.LogInformation("Adding role {0} {1}ms", RoleName, timer.ElapsedMilliseconds);
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
                }
            }
            await CheckRoleExist(Role.Administrator);
            await CheckRoleExist(Role.User);
            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                _Logger.LogInformation("Adding administrator...");
                var admin = new User { UserName = User.Administrator };
                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded)
                {
                    _Logger.LogInformation("Administrator added successfully");
                    var role_arr_result = await _UserManager.AddToRoleAsync(admin, Role.Administrator);
                    if (role_arr_result.Succeeded)
                        _Logger.LogInformation("Administrator role was added to the administrator successfully");
                    else
                    {
                        var error = string.Join(",", role_arr_result.Errors.Select(e => e.Description));
                        _Logger.LogError("Error adding the Administrator role to the administrator {0}", error);
                        throw new InvalidOperationException($"Error adding the Administrator role to the administrator: {error}");
                    }
                }
                else
                {
                    var errors = string.Join(", ", creation_result.Errors.Select(e => e.Description));
                    _Logger.LogError("Error creating user Administrator {0}", errors);
                    throw new InvalidOperationException($"Error creating user Administrator: {string.Join(", ", errors)}");
                }
            }
        }
    }
}
