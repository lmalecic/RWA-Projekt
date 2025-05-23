﻿using AutoMapper;
using DAL.DTO;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly BookLibraryContext _context;
        private readonly IMapper _mapper;
        private readonly IEntityService<Location> _locationService;

        public LocationController(BookLibraryContext _context, IMapper _mapper, IEntityService<Location> locationService)
        {
            this._context = _context;
            this._mapper = _mapper;
            this._locationService = locationService;
        }

        // GET: api/<LocationController>
        [HttpGet]
        public IActionResult Get()
        {
            try {
                var result = _locationService.GetAll()
                    .Select(_mapper.Map<LocationDto>);
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<LocationController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try {
                var result = _locationService.Get(id);
                var mappedResult = _mapper.Map<LocationDto>(result);
                return Ok(mappedResult);
            }
            catch (FileNotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<LocationController>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] LocationUpdateDto? updateDto)
        {
            if (updateDto == null) {
                return BadRequest("Location data is null.");
            }

            try {
                var entity = _locationService.Create(_mapper.Map<Location>(updateDto));
                var mapped = _mapper.Map<LocationDto>(entity);
                var location = Url.Action(nameof(Get), new { id = entity.Id });
                return Created(location, mapped);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<LocationController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] LocationUpdateDto updateDto)
        {
            try {
                var existing = _locationService.Update(id, updateDto);
                var mapped = _mapper.Map<LocationDto>(existing);
                return Ok(mapped);
            }
            catch (FileNotFoundException ex) { 
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<LocationController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try {
                var entity = _locationService.Delete(id);
                var mapped = _mapper.Map<LocationDto>(entity);
                return Ok(mapped);
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