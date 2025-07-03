using AutoMapper;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReservationController : Controller
    {
        private readonly UserReservationService _reservationService;
        private readonly UserService _userService;
        private readonly BookService _bookService;
        private readonly IMapper _mapper;

        public ReservationController(UserReservationService reservationService, UserService userService, IMapper mapper, BookService bookService)
        {
            _reservationService = reservationService;
            _userService = userService;
            _mapper = mapper;
            _bookService = bookService;
        }

        public IActionResult Index()
        {
            var reservations = _reservationService.GetAll();
            var viewModels = reservations.Select(_mapper.Map<ReservationEditViewModel>);
            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Create(int userId)
        {
            var user = _userService.Get(userId);
            var viewModel = new ReservationCreateViewModel {
                UserId = userId,
                User = _mapper.Map<UserViewModel>(user),
                Date = DateTime.Now,
                AvailableBooks = _bookService.GetAll()
                    .Select(_mapper.Map<BookViewModel>).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReservationCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try {
                var reservation = _mapper.Map<UserReservation>(viewModel);
                _reservationService.Create(reservation);
                return RedirectToAction("Edit", "User", new { id = viewModel.UserId });
            }
            catch {
                ModelState.AddModelError("", "Failed to create reservation");
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try {
                var reservation = _reservationService.Get(id);
                var viewModel = _mapper.Map<ReservationEditViewModel>(reservation);
                return View(viewModel);
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ReservationEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            try
            {
                var reservation = _reservationService.Get(viewModel.Id);
                _mapper.Map(viewModel, reservation);
                _reservationService.Update(reservation);
                return RedirectToAction("Edit", "User", new { id = viewModel.UserId });
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Failed to update reservation");
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var reservation = _reservationService.Get(id);
                var viewModel = _mapper.Map<ReservationEditViewModel>(reservation);
                return View(viewModel);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, int userId)
        {
            try {
                _reservationService.Delete(id);
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