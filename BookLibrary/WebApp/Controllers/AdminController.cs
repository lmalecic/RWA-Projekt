using AutoMapper;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly BookService _bookService;
        private readonly UserService _userService;
        private readonly IEntityService<Genre> _genreService;
        private readonly IEntityService<Location> _locationService;
        private readonly IMapper _mapper;

        public AdminController(
            BookService bookService,
            UserService userService,
            IMapper mapper, 
            IEntityService<Genre> genreService,
            IEntityService<Location> locationService)
        {
            this._bookService = bookService;
            this._mapper = mapper;
            this._genreService = genreService;
            this._locationService = locationService;
            this._userService = userService;
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
            var genres = _genreService.GetAll().Select(_mapper.Map<GenreViewModel>);
            return View(genres);
        }

        [HttpGet("[action]")]
        public IActionResult Locations()
        {
            var locations = _locationService.GetAll().Select(_mapper.Map<LocationViewModel>);
            return View(locations);
        }

        [HttpGet("[action]")]
        public IActionResult Users()
        {
            var users = _userService.GetAll().Select(_mapper.Map<UserViewModel>);
            return View(users);
        }
    }
}
