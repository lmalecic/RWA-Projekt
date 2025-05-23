﻿using AutoMapper;
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
        private readonly IEntityService<Genre> _genreService;

        public GenreController(BookLibraryContext _context, IMapper _mapper, IEntityService<Genre> genreService)
        {
            this._context = _context;
            this._mapper = _mapper;
            this._genreService = genreService;
        }

        // GET: api/<GenreController>
        [HttpGet]
        public IActionResult Get()
        {
            try {
                var result = _genreService.GetAll();
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
                var result = _genreService.Get(id);
                var mappedResult = _mapper.Map<GenreDto>(result);
                return Ok(mappedResult);
            }
            catch (FileNotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<GenreController>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] GenreUpdateDto? updateDto)
        {
            if (updateDto == null) {
                return BadRequest("Genre data is null.");
            }

            try {
                var genre = _genreService.Create(_mapper.Map<Genre>(updateDto));
                var mapped = _mapper.Map<GenreDto>(genre);
                var location = Url.Action(nameof(Get), new { id = genre.Id });

                return Created(location, mapped);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<GenreController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] GenreUpdateDto genreUpdateDto)
        {
            try {
                var existingGenre = _genreService.Update(id, genreUpdateDto);
                return Ok(_mapper.Map<GenreDto>(existingGenre));
            }
            catch (FileNotFoundException ex) {
                return NotFound(ex.Message);
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
            try {
                var deleted = _genreService.Delete(id);
                return Ok(_mapper.Map<GenreDto>(deleted));
            }
            catch (BadHttpRequestException ex) {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
