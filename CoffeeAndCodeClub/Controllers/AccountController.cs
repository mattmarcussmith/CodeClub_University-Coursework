using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeCodeCoffee.Data;
using WeCodeCoffee.Models;
using WeCodeCoffee.ViewModel;

namespace WeCodeCoffee.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var email = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (email == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email");
                return View(loginViewModel);
            }
            var passwordCheck = await _userManager.CheckPasswordAsync(email, loginViewModel.Password);
            if (!passwordCheck)
            {
                ModelState.AddModelError(string.Empty, "Invalid Password");
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(email, loginViewModel.Password, false, false);
            var roles = await _userManager.GetRolesAsync(email);
            if (result.Succeeded)
            {


                return RedirectToAction("Index", "Home");

            }

            ModelState.AddModelError(string.Empty, "Login failed. Please try again");
            return View(loginViewModel);

        }


        [HttpGet]
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                ModelState.AddModelError("EmailAddress", "Email already in use");
                return View(registerViewModel);
            }
            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,

            };
            var newUserRegister = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            if (newUserRegister.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return RedirectToAction("Login", "Account");

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

     
    }
}

