using AutoMapper;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GenreController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEntityService<Genre> _genreService;

        public GenreController(IMapper mapper, IEntityService<Genre> genreService)
        {
            _mapper = mapper;
            _genreService = genreService;
        }

        // GET: GenreController/Details/5
        public ActionResult Details(int id)
        {
            try {
                var genre = _genreService.Get(id);
                var genreViewModel = _mapper.Map<Models.GenreViewModel>(genre);
                return View(genreViewModel);
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
        }

        // GET: GenreController/Create
        public ActionResult Create()
        {
            return View(new GenreViewModel());
        }

        // POST: GenreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GenreViewModel createViewModel)
        {
            if (!ModelState.IsValid) {
                return View(createViewModel);
            }

            try {
                var genre = _mapper.Map<Genre>(createViewModel);
                var created = _genreService.Create(genre);
                return RedirectToAction(nameof(Details), new { id =  created.Id});
            }
            catch (Exception) {
                ModelState.AddModelError("", "An error occurred while creating the genre. Please try again.");
                return View(createViewModel);
            }
        }

        // GET: GenreController/Edit/5
        public ActionResult Edit(int id)
        {
            try {
                var genre = _genreService.Get(id);
                var genreViewModel = _mapper.Map<GenreViewModel>(genre);
                return View(genreViewModel);
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
            catch (Exception) {
                return StatusCode(500, "An error occurred while retrieving the genre for editing.");
            }
        }

        // POST: GenreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GenreViewModel editViewModel)
        {
            try {
                var genre = _mapper.Map<Genre>(editViewModel);
                var updated = _genreService.Update(genre);
                return RedirectToAction(nameof(Details), updated);
            }
            catch {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: GenreController/Delete/5
        public ActionResult Delete(int id)
        {
            try {
                var genre = _genreService.Get(id);
                var genreViewModel = _mapper.Map<GenreViewModel>(genre);
                return View(genreViewModel);
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
        public ActionResult Delete(GenreViewModel deleteViewModel)
        {
            try {
                _genreService.Delete(deleteViewModel.Id);
                return RedirectToAction(nameof(AdminController.Genres), "Admin");
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
