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
        private readonly BookService _bookService;

        public BookController(BookLibraryContext _context, IMapper _mapper, LogService logService, BookService bookService)
        {
            this._context = _context;
            this._mapper = _mapper;
            this._logService = logService;
            this._bookService = bookService;
        }

        // GET: api/<BookController>
        [HttpGet]
        public IActionResult Get()
        {
            try {
                var all = _bookService.GetAll();
                var mapped = _mapper.Map<IEnumerable<BookDto>>(all);
                return Ok(mapped);
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
                var result = _bookService.Get(id);
                if (result == null) {
                    return NotFound($"Book with id {id} not found.");
                }

                var mappedResult = _mapper.Map<BookDto>(result);
                return Ok(mappedResult);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while getting id={id}.", 3);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public IActionResult Search(int count = 10, int page = 1, string? name = null, string? author = null, int? genreId = null, string? description = null)
        {
            try {
                var result = _bookService.Search(new() {
                    Count = count,
                    Page = page,
                    Name = name,
                    Author = author,
                    Description = description,
                    GenreId = genreId
                });

                _mapper.Map<IEnumerable<BookDto>>(result.Results);

                return Ok(result);
            }
            catch (BadHttpRequestException ex)
            {
                _logService.Log("A user has occurred while searching books.", 1);
                return BadRequest(ex.Message);
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
                _bookService.Create(book); // Use the BookService to create the book
                bookDto.Id = book.Id; // Set the ID of the newly created book in the DTO

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
        public IActionResult Put(int id, [FromBody] BookUpdateDto updateDto)
        {
            try {
                var book = _bookService.Update(id, updateDto);
                if (book == null) {
                    return NotFound($"Book with ID {id} not found.");
                }

                return Ok(_mapper.Map<BookDto>(book));
            }
            catch (BadHttpRequestException ex) {
                _logService.Log($"An error has occurred while updating book with id={id}.", 2);
                return StatusCode(ex.StatusCode, ex.Message);
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
                patchDoc.ApplyTo(patched);

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
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try {
                var entity = _bookService.Delete(id);
                if (entity == null) {
                    return NoContent();
                }

                return Ok(_mapper.Map<BookDto>(entity));
            }
            catch (BadHttpRequestException ex) {
                _logService.Log($"An error has occurred while deleting book with id={id}.", 3);
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while deleting book with id={id}.", 3);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
