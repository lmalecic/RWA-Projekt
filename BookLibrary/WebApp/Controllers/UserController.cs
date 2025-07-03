using AspNetCoreGeneratedDocument;
using AutoMapper;
using DAL.Models;
using DAL.Services;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

using WebApp.Models;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly BookLibraryContext _context;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly UserReservationService _reservationService;
        private readonly UserReviewService _reviewService;

        public UserController(
            BookLibraryContext context, 
            IMapper mapper, 
            UserService userService,
            UserReservationService reservationService,
            UserReviewService reviewService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _reservationService = reservationService;
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl)
        {
            // User already authenticated, redirect to home or return URL
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return returnUrl != null ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Home");
            }

            var userViewModel = new UserRegisterViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(userViewModel);
        }

        [HttpPost]
        public IActionResult Register(UserRegisterViewModel userViewModel)
        {
            try
            {
                // Check if there is such a username in the database already
                var trimmedUsername = userViewModel.Username.Trim();
                if (_context.Users.Any(x => x.Username.Equals(trimmedUsername)))
                {
                    ModelState.AddModelError("", $"Username {trimmedUsername} already exists!");
                    return View();
                }

                // Create user from DTO and hashed password
                var user = _mapper.Map<User>(userViewModel);
                user.PwdHash = Argon2.Hash(userViewModel.Password);

                // Add user and save changes to database
                _context.Users.Add(user);
                _context.SaveChanges();

                return View("RegisterSuccess", new UserRegisterViewModel { ReturnUrl = userViewModel.ReturnUrl ?? Url.Action("Index", "Home") });

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            // User already authenticated, redirect to home or return URL
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return returnUrl != null ? LocalRedirect(returnUrl) : RedirectToAction("Index", "Home");
            }

            var userViewModel = new UserLoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel loginViewModel)
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
                new(ClaimTypes.Name, loginViewModel.Username),
                new(ClaimTypes.Role, existingUser.Role)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

            if (loginViewModel.ReturnUrl != null)
                return LocalRedirect(loginViewModel.ReturnUrl);
            else if (existingUser.Role == "Admin")
                return RedirectToAction("Index", "Admin");
            else if (existingUser.Role == "User")
                return RedirectToAction("Index", "Home");
            else
                return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        public ActionResult Details(int id)
        {
            try {
                var dbEntity = _userService.Get(id);
                var entityViewModel = _mapper.Map<UserViewModel>(dbEntity);
                var reservations = dbEntity.UserReservations.Select(_mapper.Map<UserReservationViewModel>);
                var reviews = dbEntity.UserReviews.Select(_mapper.Map<UserReviewViewModel>);

                return View(new UserDetailsViewModel {
                    User = entityViewModel,
                    Reservations = reservations,
                    Reviews = reviews
                });
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
        }

        // GET: GenreController/Edit/5
        public ActionResult Edit(int id)
        {
            try {
                var dbEntity = _userService.Get(id);  // This already includes Reservations and Reviews with Books
                var viewModel = new UserDetailsViewModel {
                    User = _mapper.Map<UserViewModel>(dbEntity),
                    Reservations = dbEntity.UserReservations.Select(_mapper.Map<UserReservationViewModel>),
                    Reviews = dbEntity.UserReviews.Select(_mapper.Map<UserReviewViewModel>)
                };
                return View(viewModel);
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
            catch (Exception) {
                return StatusCode(500, "An error occurred while retrieving the user for editing.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel editViewModel)
        {
            try {
                var dbEntity = _mapper.Map<User>(editViewModel);
                var updated = _userService.Update(dbEntity);
                return RedirectToAction(nameof(Details), updated);
            }
            catch {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public ActionResult DeleteReservation(int userId, int reservationId)
        {
            try
            {
                var reservation = _reservationService.Get(reservationId);
                
                if (reservation == null || reservation.UserId != userId)
                    return NotFound();

                _reservationService.Delete(reservationId);
                
                return RedirectToAction(nameof(Edit), new { id = userId });
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the reservation.");
            }
        }

        [HttpPost]
        public ActionResult DeleteReview(int userId, int reviewId)
        {
            try
            {
                var review = _reviewService.Get(reviewId);
                
                if (review == null || review.UserId != userId)
                    return NotFound();

                _reviewService.Delete(reviewId);
                
                return RedirectToAction(nameof(Edit), new { id = userId });
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the review.");
            }
        }

        // GET: GenreController/Delete/5
        public ActionResult Delete(int id)
        {
            try {
                var dbEntity = _userService.Get(id);
                var viewModel = _mapper.Map<UserViewModel>(dbEntity);
                return View(viewModel);
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
            catch (Exception) {
                return StatusCode(500, "An error occurred while retrieving the genre for deletion.");
            }
        }

        // POST: GenreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserViewModel deleteViewModel)
        {
            try {
                _userService.Delete(deleteViewModel.Id);
                return RedirectToAction(nameof(AdminController.Users), "Admin");
            }
            catch (BadHttpRequestException ex) {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(deleteViewModel);
            }
            catch (Exception ex) {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
