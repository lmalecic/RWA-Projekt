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

        [HttpGet("[action]")]
        public IActionResult Index()
        {
            return RedirectToAction("Books");
        }

        [HttpGet("[action]")]
        public IActionResult Books([FromQuery] BookSearchParams searchParams)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var searchResult = _bookService.Search(searchParams);
            var mapped = _mapper.Map<SearchResult<BookViewModel>>(searchResult);

            return View(new BooksViewModel {
                SearchResult = mapped,
                Genres = _genreService.GetAll().Select(_mapper.Map<GenreViewModel>)
            });
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
