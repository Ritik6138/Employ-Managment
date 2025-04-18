using Employee.Repository.Models;
using Employee.Service.Interfaces;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor to initialize the authentication service.
        /// </summary>
        /// Created by: Ritik Kumar
        public AuthController(IAuthService authService) => _authService = authService;

        /// <summary>
        /// This method is used to show the registration page.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <returns>Returns the registration view.</returns>
        [HttpGet]
        public IActionResult Register() => View();

        /// <summary>
        /// This method handles user registration process.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <param name="model">The registration view model containing user information.</param>
        /// <returns>Redirects to Login on success or returns the registration view on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User { Username = model.Username, Email = model.Email };
            var result = await _authService.RegisterAsync(user, model.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Registration successful. Please log in to continue.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);

            return View(model);
        }

        /// <summary>
        /// This method is used to show the login page.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <returns>Returns the login view.</returns>
        [HttpGet]
        public IActionResult Login() => View();

        /// <summary>
        /// This method handles user authentication.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <param name="model">The login view model containing user credentials.</param>
        /// <returns>Redirects to Employee index on success or returns the login view on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _authService.LoginAsync(model.Email, model.Password);

            if (result.Succeeded)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.User.Username),
                    new Claim(ClaimTypes.Email, result.User.Email)
                };

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Invalid email or password. Please try again.");
            return View(model);
        }

        /// <summary>
        /// This method handles user logout.
        /// </summary>
        /// Created by: Ritik Kumar
        /// <returns>Redirects to the login page.</returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
