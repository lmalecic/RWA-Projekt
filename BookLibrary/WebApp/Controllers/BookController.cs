using AutoMapper;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _bookService;
        private readonly IEntityService<Genre> _genreService;
        private readonly IMapper _mapper;

        public BookController(BookService bookService, IMapper mapper, IEntityService<Genre> genreService)
        {
            this._bookService = bookService;
            this._mapper = mapper;
            this._genreService = genreService;
        }

        // GET: BooksController
        // Non-admin view of books
        public ActionResult Index()
        {
            var searchParams = new BookSearchParams
            {
                Page = 1,
                Count = 10 // Default page size
            };

            var searchResult = _bookService.Search(searchParams);
            var viewModel = new BooksViewModel
            {
                SearchResult = new SearchResult<BookViewModel>(
                    searchResult.Count,
                    searchResult.Page,
                    searchResult.TotalPages,
                    _mapper.Map<IEnumerable<BookViewModel>>(searchResult.Items)
                ),
                Genres = _genreService.GetAll().Select(_mapper.Map<GenreViewModel>)
            };

            return View(viewModel);
        }

        // GET: BooksController/Details/5
        // View of book details (both admin and non-admin)
        public ActionResult Details(int id, string? returnUrl)
        {
            try {
                var dbBook = _bookService.Get(id);
                var book = _mapper.Map<BookViewModel>(dbBook);

                return View(new BookDetailsViewModel {
                    Book = book,
                    ReturnUrl = returnUrl
                });
            }
            catch (FileNotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: BooksController/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var model = new BookCreateViewModel {
                Book = new BookViewModel(),
                ExistingGenres = _genreService.GetAll().Select(_mapper.Map<GenreViewModel>),
            };

            return View(model);
        }

        // POST: BooksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(BookCreateViewModel createViewModel)
        {
            try {
                var book = _mapper.Map<Book>(createViewModel.Book);
                _bookService.Create(book);

                var mapped = _mapper.Map<BookViewModel>(book);
                return RedirectToAction(nameof(Details), new { id = mapped.Id, returnUrl = $"Admin/{nameof(AdminController.Books)}" });
            }
            catch (Exception) {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: BooksController/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try {
                var dbBook = _bookService.Get(id);
                var book = _mapper.Map<BookViewModel>(dbBook);
                return View(new BookEditViewModel {
                    Book = book,
                    ExistingGenres = _genreService.GetAll().Select(_mapper.Map<GenreViewModel>)
                });
            }
            catch (FileNotFoundException ex) {
                return NotFound();
            }
            catch (Exception ex) {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: BooksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(BookEditViewModel editViewModel)
        {
            try {
                var book = _mapper.Map<Book>(editViewModel.Book);
                _bookService.Update(book);

                return RedirectToAction(nameof(Details), new { id = book.Id, returnUrl = editViewModel.ReturnUrl });
            }
            catch (FileNotFoundException ex) {
                return NotFound();
            }
            catch (Exception) {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: BooksController/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try {
                var dbBook = _bookService.Get(id);
                var book = _mapper.Map<BookViewModel>(dbBook);
                return View(new BookDeleteViewModel {
                    Book = book,
                    ReturnUrl = $"~/Admin/{nameof(AdminController.Books)}"
                });
            }
            catch (FileNotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: BooksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(BookDeleteViewModel deleteViewModel)
        {
            try {
                _bookService.Delete(deleteViewModel.Book.Id);
                return deleteViewModel.ReturnUrl != null ?
                    Redirect(deleteViewModel.ReturnUrl) :
                    RedirectToAction(nameof(Index));
            }
            catch (BadHttpRequestException ex) {
                // This exception is thrown when the book is used in reservations, reviews, or locations
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(deleteViewModel);
            }
            catch (Exception ex) {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public IActionResult Search([FromQuery] BookSearchParams searchParams)
        {
            var searchResult = _bookService.Search(searchParams);
            var viewModel = new BooksViewModel
            {
                SearchResult = new SearchResult<BookViewModel>(
                    searchResult.Count,
                    searchResult.Page,
                    searchResult.TotalPages,
                    _mapper.Map<IEnumerable<BookViewModel>>(searchResult.Items)
                ),
                Genres = _genreService.GetAll().Select(_mapper.Map<GenreViewModel>)
            };
            
            return PartialView("_BooksTable", viewModel);
        }
    }
}
