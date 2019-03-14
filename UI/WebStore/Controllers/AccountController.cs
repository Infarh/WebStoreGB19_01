using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SingInManager;
        private readonly ILogger<AccountController> _Logger;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> singInManager,
            ILogger<AccountController> logger)
        {
            _UserManager = userManager;
            _SingInManager = singInManager;

            _Logger = logger;
        }

        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            _Logger.LogInformation(new EventId(0, "Login"), 
                $"Попытка входа пользователя {model.UserName} в систему");

            var login_result = await _SingInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                false);

            if (login_result.Succeeded)
            {
                _Logger.LogInformation(new EventId(1, "Login"),
                    $"Пользователь {model.UserName} успешно вошел в систему");

                if (Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _Logger.LogWarning(new EventId(1, "Login"),
                    $"Попытка входа пользователя {model.UserName} неуспешная");
            }

            ModelState.AddModelError("", "Неверное имя, или пароль пользователя");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User
            {
                UserName = model.UserName
            };

            var registration_result = await _UserManager.CreateAsync(user, model.Password);
            if (registration_result.Succeeded)
            {
                await _UserManager.AddToRoleAsync(user, Domain.Entities.User.UserRole);
                await _SingInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var identity_error in registration_result.Errors)
                ModelState.AddModelError("", identity_error.Description);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _SingInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}