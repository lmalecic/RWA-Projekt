using AutoMapper;
using DAL.DTO;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly BookLibraryContext _context;
        private readonly IMapper _mapper;

        public GenreController(BookLibraryContext _context, IMapper _mapper)
        {
            this._context = _context;
            this._mapper = _mapper;
        }

        // GET: api/<GenreController>
        [HttpGet]
        public IActionResult Get()
        {
            try {
                var result = _context.Genres;
                var mappedResult = _mapper.Map<IEnumerable<GenreDto>>(result);
                return Ok(mappedResult);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try {
                var result = _context.Genres
                    .FirstOrDefault(x => x.Id == id);

                if (result == null) {
                    return NotFound($"Genre with id {id} not found.");
                }

                var mappedResult = _mapper.Map<GenreDto>(result);

                return Ok(mappedResult);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<GenreController>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] GenreDto genreDto)
        {
            try {
                var genre = _mapper.Map<Genre>(genreDto);

                _context.Genres.Add(genre);
                _context.SaveChanges();

                genreDto.Id = genre.Id; // Set the ID of the newly created genre

                var location = Url.Action(nameof(Get), new { id = genre.Id });
                return Created(location, genreDto);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<GenreController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] GenreDto genreDto)
        {
            if (genreDto == null) {
                return BadRequest("Invalid genre data.");
            }

            var existingGenre = _context.Genres.FirstOrDefault(x => x.Id == id);
            if (existingGenre == null) {
                return NotFound($"Genre with ID {id} not found.");
            }

            try {
                _mapper.Map(genreDto, existingGenre);
                _context.SaveChanges();
                return Ok(_mapper.Map<GenreDto>(existingGenre));
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<GenreController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingGenre = _context.Genres.FirstOrDefault(x => x.Id == id);
            if (existingGenre == null) {
                return NoContent();
            }

            // Check if the genre is used in any book
            var isUsedInBooks = _context.Books.Any(b => b.GenreId == id);
            if (isUsedInBooks) {
                return BadRequest("Cannot delete genre that is used in books.");
            }

            try {
                _context.Genres.Remove(existingGenre);
                _context.SaveChanges();

                return Ok(_mapper.Map<GenreDto>(existingGenre));
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
