using AutoMapper;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _bookService;
        private readonly IMapper _mapper;

        public BookController(BookService bookService, IMapper mapper)
        {
            this._bookService = bookService;
            this._mapper = mapper;
        }

        // GET: BooksController
        public ActionResult Index()
        {
            var dbBooks = _bookService.GetAll();
            var books = _mapper.Map<IEnumerable<BookViewModel>>(dbBooks);

            return View(books);
        }

        // GET: BooksController/Details/5
        public ActionResult Details(int id)
        {
            try {
                var dbBook = _bookService.Get(id);
                if (dbBook == null) {
                    return NotFound();
                }
                var book = _mapper.Map<BookViewModel>(dbBook);
                return View(book);
            }
            catch (FileNotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: BooksController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(new BookCreateViewModel());
        }

        // POST: BooksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(IFormCollection collection)
        {
            try {
                return RedirectToAction(nameof(Index));
            }
            catch {
                return View();
            }
        }

        // GET: BooksController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try {
                var dbBook = _bookService.Get(id);
                var book = _mapper.Map<BookViewModel>(dbBook);
                return View(book);
            }
            catch (FileNotFoundException ex) {
                return NotFound();
            }
        }

        // POST: BooksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try {
                return RedirectToAction(nameof(Index));
            }
            catch {
                return View();
            }
        }

        // GET: BooksController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try {
                var dbBook = _bookService.Get(id);
                if (dbBook == null) {
                    return NotFound();
                }

                var book = _mapper.Map<BookViewModel>(dbBook);
                return RedirectToAction("Index", "Book");
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
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try {
                _bookService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (BadHttpRequestException ex) {
                return BadRequest(ex.Message);
            }
            catch {
                return View();
            }
        }
    }
}
