using AutoMapper;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly BookService _bookService;
        private readonly IEntityService<Genre> _genreService;
        private readonly IEntityService<Location> _locationService;
        private readonly IMapper _mapper;

        public AdminController(
            BookService bookService, 
            IMapper mapper, 
            IEntityService<Genre> genreService,
            IEntityService<Location> locationService)
        {
            this._bookService = bookService;
            this._mapper = mapper;
            this._genreService = genreService;
            this._locationService = locationService;
        }

        [HttpGet("")]
        [HttpGet("[action]")]
        public IActionResult Index()
        {
            return RedirectToAction("Books");
        }

        [HttpGet("[action]")]
        public IActionResult Books(string? name, string? author, string? description, int? genreId, int page = 1, int count = 10)
        {
            var searchParams = new BookSearchParams() {
                Count = count,
                Page = page
            };
            
            if (!string.IsNullOrWhiteSpace(name))
                searchParams.Name = name;
                
            if (!string.IsNullOrWhiteSpace(author))
                searchParams.Author = author;
                
            if (!string.IsNullOrWhiteSpace(description))
                searchParams.Description = description;
                
            if (genreId.HasValue)
                searchParams.GenreId = genreId;
            
            var searchResult = _bookService.Search(searchParams);
            var books = _mapper.Map<IEnumerable<BookViewModel>>(searchResult.Results);
            
            ViewBag.CurrentPage = searchResult.Page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)searchResult.Total / searchResult.Count);
            ViewBag.TotalItems = searchResult.Total;
            ViewBag.ItemsPerPage = searchResult.Count;

            return View(nameof(Books), books);
        }

        [HttpGet("[action]")]
        public IActionResult Genres()
        {
            var dbGenres = _genreService.GetAll();
            var genres = _mapper.Map<IEnumerable<GenreViewModel>>(dbGenres);

            return View();
        }

        [HttpGet("[action]")]
        public IActionResult Locations()
        {
            return View();
        }

        [HttpGet("[action]")]
        public IActionResult Users()
        {
            return View();
        }
    }
}
