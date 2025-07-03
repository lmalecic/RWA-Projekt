using AutoMapper;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LocationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEntityService<Location> _locationService;

        public LocationController(IMapper mapper, IEntityService<Location> locationService)
        {
            _mapper = mapper;
            _locationService = locationService;
        }

        // GET: LocationController/Details/5
        public ActionResult Details(int id)
        {
            try {
                var dbEntity = _locationService.Get(id);
                var viewModel = _mapper.Map<LocationViewModel>(dbEntity);
                return View(viewModel);
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
        }

        // GET: LocationController/Create
        public ActionResult Create()
        {
            return View(new LocationViewModel());
        }

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocationViewModel createViewModel)
        {
            if (!ModelState.IsValid) {
                return View(createViewModel);
            }

            try {
                var entity = _mapper.Map<Location>(createViewModel);
                var created = _locationService.Create(entity);
                return RedirectToAction(nameof(Details), new { id = created.Id });
            }
            catch (Exception) {
                ModelState.AddModelError("", "An error occurred while creating the location. Please try again.");
                return View(createViewModel);
            }
        }

        // GET: LocationController/Edit/5
        public ActionResult Edit(int id)
        {
            try {
                var dbEntity = _locationService.Get(id);
                var viewModel = _mapper.Map<LocationViewModel>(dbEntity);
                return View(viewModel);
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
            catch (Exception) {
                return StatusCode(500, "An error occurred while retrieving the genre for editing.");
            }
        }

        // POST: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LocationViewModel editViewModel)
        {
            try {
                var entity = _mapper.Map<Location>(editViewModel);
                var updated = _locationService.Update(entity);
                return RedirectToAction(nameof(Details), updated);
            }
            catch {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: LocationController/Delete/5
        public ActionResult Delete(int id)
        {
            try {
                var dbEntity = _locationService.Get(id);
                var viewModel = _mapper.Map<LocationViewModel>(dbEntity);
                return View(viewModel);
            }
            catch (FileNotFoundException) {
                return NotFound();
            }
            catch (Exception) {
                return StatusCode(500, "An error occurred while retrieving the genre for deletion.");
            }
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(LocationViewModel deleteViewModel)
        {
            try {
                _locationService.Delete(deleteViewModel.Id);
                return RedirectToAction(nameof(AdminController.Locations), "Admin");
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
