using AutoMapper;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReviewController : Controller
    {
        private readonly UserReviewService _reviewService;
        private readonly UserService _userService;
        private readonly BookService _bookService;
        private readonly IMapper _mapper;

        public ReviewController(UserReviewService reviewService, UserService userService, BookService bookService, IMapper mapper)
        {
            _reviewService = reviewService;
            _userService = userService;
            _mapper = mapper;
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            var reviews = _reviewService.GetAll();
            var viewModels = reviews.Select(_mapper.Map<ReviewEditViewModel>);
            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Create(int userId)
        {
            var user = _userService.Get(userId);
            var viewModel = new ReviewCreateViewModel {
                UserId = userId,
                User = _mapper.Map<UserViewModel>(user),
                AvailableBooks = _bookService.GetAll()
                    .Select(_mapper.Map<BookViewModel>).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReviewCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try {
                var review = new UserReview {
                    UserId = viewModel.UserId,
                    BookId = viewModel.BookId,
                    Rating = viewModel.Rating,
                    Text = viewModel.Text
                };

                _reviewService.Create(review);
                return RedirectToAction("Edit", "User", new { id = viewModel.UserId });
            }
            catch {
                ModelState.AddModelError("", "Failed to create review");
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try {
                var review = _reviewService.Get(id);
                var viewModel = _mapper.Map<ReviewEditViewModel>(review);
                return View(viewModel);
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ReviewEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                var review = _reviewService.Get(viewModel.Id);
                _mapper.Map(viewModel, review);
                _reviewService.Update(review);
                return RedirectToAction("Edit", "User", new { id = viewModel.UserId });
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                ModelState.AddModelError("", "Failed to update review");
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try {
                var review = _reviewService.Get(id);
                var viewModel = _mapper.Map<ReviewEditViewModel>(review);
                return View(viewModel);
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, int userId)
        {
            try {
                _reviewService.Delete(id);
                return RedirectToAction("Edit", "User", new { id = userId });
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
            catch {
                return RedirectToAction("Edit", "User", new { id = userId });
            }
        }
    }
}