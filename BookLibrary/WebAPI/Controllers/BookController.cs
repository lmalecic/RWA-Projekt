using AutoMapper;
using DAL.DTO;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookLibraryContext _context;
        private readonly IMapper _mapper;
        private readonly LogService _logService;

        public BookController(BookLibraryContext _context, IMapper _mapper, LogService logService)
        {
            this._context = _context;
            this._mapper = _mapper;
            this._logService = logService;
        }

        // GET: api/<BookController>
        [HttpGet]
        public IActionResult Get()
        {
            try {
                var result = _context.Books;
                var mappedResult = _mapper.Map<IEnumerable<BookDto>>(result);

                _logService.Log($"Read all Books.", 0);
                return Ok(mappedResult);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while getting all books.", 3);
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<BookController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try {
                var result = _context.Books
                    .FirstOrDefault(x => x.Id == id);

                if (result == null) {
                    _logService.Log($"Could not read Book with id={id}; not found.", 1);
                    return NotFound($"Book with id {id} not found.");
                }

                var mappedResult = _mapper.Map<BookDto>(result);

                _logService.Log($"Read Book with id={id}", 0);
                return Ok(mappedResult);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while getting id={id}.", 3);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public IActionResult Search(int count = 10, int? page = 1, string? name = null, string? author = null, int? genreId = null, string? description = null)
        {
            try {
                if (page < 1 || count < 1) {
                    _logService.Log($"Could not search books; page and count must be positive integers.", 1);
                    return BadRequest("Page and count must be positive integers.");
                }

                var query = _context.Books.AsQueryable();
                
                if (genreId != null)
                    query = query.Where(b => b.GenreId == genreId);

                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(b => b.Name.Contains(name));

                if (!string.IsNullOrWhiteSpace(author))
                    query = query.Where(b => b.Author.Contains(author));

                if (!string.IsNullOrWhiteSpace(description))
                    query = query.Where(b => b.Description != null ? b.Description.Contains(description) : false);

                var total = query.Count();

                var books = query
                    .Skip(((page ?? 1) - 1) * count)
                    .Take(count)
                    .ToList();

                var mappedBooks = _mapper.Map<IEnumerable<BookDto>>(books);

                _logService.Log($"Searched books (name: {name}, author: {author}, page: {page}, count: {count})", 0);

                return Ok(new {
                    Total = total,
                    Page = page ?? 1,
                    Count = count,
                    Results = mappedBooks
                });
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while searching books.", 3);
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<BookController>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] BookDto bookDto)
        {
            try {
                var book = _mapper.Map<Book>(bookDto);

                _context.Books.Add(book);
                _context.SaveChanges();

                bookDto.Id = book.Id; // Set the ID of the newly created book

                _logService.Log($"Created Book with id={book.Id}", 0);
                var location = Url.Action(nameof(Get), new { id = book.Id });
                return Created(location, bookDto);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while creating a new book.", 3);
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<BookController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] BookDto bookDto)
        {
            if (bookDto == null) {
                _logService.Log($"Could not update Book with id={id}; Invalid book data.", 1);
                return BadRequest("Invalid book data.");
            }

            var existingBook = _context.Books.FirstOrDefault(x => x.Id == id);
            if (existingBook == null) {
                _logService.Log($"Could not update Book with id={id}; not found.", 1);
                return NotFound($"Book with ID {id} not found.");
            }

            try {
                _mapper.Map(bookDto, existingBook);
                _context.SaveChanges();
                _logService.Log($"Updated Book with id={id}", 0);
                return Ok(_mapper.Map<BookDto>(existingBook));
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while updating book with id={id}.", 3);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument<BookPatchDto> patchDoc)
        {
            if (patchDoc == null) {
                _logService.Log($"Could not update Book with id={id}; Invalid patch document.", 1);
                return BadRequest("Invalid patch document.");
            }
            var dbBook = _context.Books.FirstOrDefault(x => x.Id == id);
            if (dbBook == null) {
                _logService.Log($"Could not update Book with id={id}; not found.", 1);
                return NotFound($"Book with ID {id} not found.");
            }
            try {
                var original = _mapper.Map<BookDto>(dbBook).Clone();
                var patched = _mapper.Map<BookPatchDto>(dbBook);
                patchDoc.ApplyTo(patched, ModelState);

                if (!TryValidateModel(ModelState)) {
                    _logService.Log($"Could not update Book with id={id}; Invalid patch document.", 1);
                    return BadRequest(ModelState);
                }

                _context.Update(_mapper.Map(patched, dbBook));
                _context.SaveChanges();

                _logService.Log($"Patched Book with id={id}", 0);
                return Ok(new {
                    Original = original,
                    Patched = patched
                });
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while patching book with id={id}.", 3);
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<BookController>/5
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingBook = _context.Books.FirstOrDefault(x => x.Id == id);
            if (existingBook == null) {
                _logService.Log($"Could not delete book with id={id}; not found.", 0);
                return NoContent();
            }

            try {
                _context.Books.Remove(existingBook);
                _context.SaveChanges();

                _logService.Log($"Deleted Book with id={id}", 0);
                return Ok(existingBook);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while deleting book with id={id}.", 3);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
