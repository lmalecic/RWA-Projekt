using AutoMapper;
using DAL.DTO;
using DAL.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly BookLibraryContext _context;
        private readonly IMapper _mapper;

        public UserController(BookLibraryContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserViewModel userViewModel)
        {
            try {
                if (!ModelState.IsValid)
                    return View();

                // Check if there is such a username in the database already
                var trimmedUsername = userViewModel.Username.Trim();
                if (_context.Users.Any(x => x.Username.Equals(trimmedUsername)))
                    return BadRequest($"Username {trimmedUsername} already exists");

                // Create user from DTO and hashed password
                var user = _mapper.Map<User>(userViewModel);
                user.PwdHash = Argon2.Hash(userViewModel.Password);

                // Add user and save changes to database
                _context.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            // User already authenticated, redirect to home or return URL
            if (User.Identity != null && User.Identity.IsAuthenticated) {
                return returnUrl != null ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Home");
            }

            var userViewModel = new UserViewModel {
                ReturnUrl = returnUrl
            };

            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginViewModel loginViewModel)
        {
            var existingUser = _context.Users
                .FirstOrDefault(x => x.Username == loginViewModel.Username);

            if (existingUser == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }

            if (!Argon2.Verify(existingUser.PwdHash, loginViewModel.Password))
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, loginViewModel.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            Task.Run(async () =>
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties)
            ).GetAwaiter().GetResult();

            if (loginViewModel.ReturnUrl != null)
                return LocalRedirect(loginViewModel.ReturnUrl);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Task.Run(async () =>
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
            ).GetAwaiter().GetResult();

            return View();
        }


    }
}
