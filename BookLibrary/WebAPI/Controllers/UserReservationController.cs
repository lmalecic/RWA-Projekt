using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DAL.DTO;
using DAL.Models;
using DAL.Services;
using System;
using System.IO;
using System.Linq;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReservationController : ControllerBase
    {
        private readonly IEntityService<UserReservation> _reservationService;
        private readonly IMapper _mapper;

        public UserReservationController(IEntityService<UserReservation> reservationService, IMapper mapper)
        {
            _reservationService = reservationService;
            _mapper = mapper;
        }

        // GET: api/UserReservation
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var reservations = _reservationService.GetAll()
                    .Select(_mapper.Map<UserReservationDto>);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/UserReservation/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var reservation = _reservationService.Get(id);
                var dto = _mapper.Map<UserReservationDto>(reservation);
                return Ok(dto);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/UserReservation
        [HttpPost]
        public IActionResult Post([FromBody] UserReservationUpdateDto reservationDto)
        {
            if (reservationDto == null)
                return BadRequest("Reservation data is null.");

            try
            {
                var reservation = _mapper.Map<UserReservation>(reservationDto);
                var created = _reservationService.Create(reservation);
                var dto = _mapper.Map<UserReservationDto>(created);
                var location = Url.Action(nameof(Get), new { id = created.Id });
                return Created(location, dto);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/UserReservation/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserReservationUpdateDto updateDto)
        {
            try
            {
                var updated = _reservationService.Update(id, updateDto);
                var dto = _mapper.Map<UserReservationDto>(updated);
                return Ok(dto);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/UserReservation/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var deleted = _reservationService.Delete(id);
                var dto = _mapper.Map<UserReservationDto>(deleted);
                return Ok(dto);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
