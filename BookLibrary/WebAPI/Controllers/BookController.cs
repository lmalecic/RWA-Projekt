﻿using AutoMapper;
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
        private readonly BookService _bookService;
        private readonly LogService _logService;

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
                var all = _bookService.GetAll()
                    .Select(_mapper.Map<BookDto>);
                return Ok(all);
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
                var mappedResult = _mapper.Map<BookDto>(result);
                return Ok(mappedResult);
            }
            catch (FileNotFoundException ex) {
                _logService.Log($"An error has occurred while getting id={id}.", 2);
                return NotFound(ex.Message);
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
        public IActionResult Post([FromBody] BookUpdateDto? bookUpdateDto)
        {
            if (bookUpdateDto == null) {
                _logService.Log($"An error has occurred while creating a new book.", 2);
                return BadRequest("Book data is null.");
            }

            try {
                var newBook = _mapper.Map<Book>(bookUpdateDto);
                var book = _bookService.Create(newBook);
                var mapped = _mapper.Map<BookDto>(book);
                var location = Url.Action(nameof(Get), new { id = book.Id });
                return Created(location, mapped);
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
                var mapped = _mapper.Map<BookDto>(book);
                return Ok(mapped);
            }
            catch (FileNotFoundException ex) {
                _logService.Log($"An error has occurred while updating book with id={id}.", 2);
                return NotFound(ex.Message);
            }
            catch (BadHttpRequestException ex) {
                _logService.Log($"An error has occurred while updating book with id={id}.", 2);
                return BadRequest(ex.Message);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while updating book with id={id}.", 3);
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
                var mapped = _mapper.Map<BookDto>(entity);
                return Ok(mapped);
            }
            catch (BadHttpRequestException ex) {
                _logService.Log($"An error has occurred while deleting book with id={id}.", 3);
                return BadRequest(ex.Message);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while deleting book with id={id}.", 3);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
