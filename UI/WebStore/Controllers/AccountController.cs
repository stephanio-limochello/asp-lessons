using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Controllers
{
	public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
		private readonly ILogger<AccountController> _logger;

		public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager, ILogger<AccountController> logger)
        {
            _userManager = UserManager;
            _signInManager = SignInManager;
			_logger = logger;
		}

        // Registration new user
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model/*, [FromServices] IMapper Mapper*/)
        {
            if (!ModelState.IsValid) return View(Model);
            using (_logger.BeginScope("Registration New User {0}", Model.UserName)) 
            {
                var user = new User
                {
                    UserName = Model.UserName
                };

                var registration_result = await _userManager.CreateAsync(user, Model.Password);
                if (registration_result.Succeeded)
                {
                    _logger.LogInformation("User {0} registered successfully", Model.UserName);

                    await _userManager.AddToRoleAsync(user, Role.User);
                    _logger.LogInformation("User {0} has been assigned a role {1}", Model.UserName, Role.User);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User {0} is automatically logged in for the first time", Model.UserName);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in registration_result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                _logger.LogWarning("Error while registering a new user {0} {1}",
                    Model.UserName,
                    string.Join(",", registration_result.Errors.Select(error => error.Description)));

            }

            return View(Model);
        }

        //  User login process
        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel { ReturnUrl = ReturnUrl });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel Model)    
        {
            if (!ModelState.IsValid) return View(Model);
            using (_logger.BeginScope("User {0} login", Model.UserName))
            {
                var login_result = await _signInManager.PasswordSignInAsync(
                     Model.UserName,
                     Model.Password,
                     Model.RememberMe,
                     lockoutOnFailure: false);
                if (login_result.Succeeded)
                {
                    _logger.LogInformation("User successfully logged in");
                    if (Url.IsLocalUrl(Model.ReturnUrl))
                    {
                        _logger.LogInformation("Redirecting the signed in user {0} to the address {1}",
                            Model.UserName, Model.ReturnUrl);
                        return Redirect(Model.ReturnUrl);
                    }
                    _logger.LogInformation("Redirecting the signed in user {0} to the home page", Model.UserName);
                    return RedirectToAction("Index", "Home");
                }
                _logger.LogWarning("Password entered error when user {0} logs on", Model.UserName);
                ModelState.AddModelError(string.Empty, "The username or password you entered is incorrect");
            }
            return View(Model);
        } 


        public async Task<IActionResult> Logout()
        {
            var user_name = User.Identity!.Name;
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User {0} is logout", user_name);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}
