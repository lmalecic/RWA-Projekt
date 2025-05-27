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
    public class UserReviewController : ControllerBase
    {
        private readonly IEntityService<UserReview> _userReviewService;
        private readonly IMapper _mapper;

        public UserReviewController(IEntityService<UserReview> userReviewService, IMapper mapper)
        {
            _userReviewService = userReviewService;
            _mapper = mapper;
        }

        // GET: api/UserReview
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var reviews = _userReviewService.GetAll()
                    .Select(_mapper.Map<UserReviewDto>);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/UserReview/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var review = _userReviewService.Get(id);
                var dto = _mapper.Map<UserReviewDto>(review);
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

        // POST: api/UserReview
        [HttpPost]
        public IActionResult Post([FromBody] UserReviewUpdateDto reviewDto)
        {
            if (reviewDto == null)
                return BadRequest("Review data is null.");

            try
            {
                var review = _mapper.Map<UserReview>(reviewDto);
                var created = _userReviewService.Create(review);
                var dto = _mapper.Map<UserReviewDto>(created);
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

        // PUT: api/UserReview/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserReviewUpdateDto updateDto)
        {
            try
            {
                var updated = _userReviewService.Update(id, updateDto);
                var dto = _mapper.Map<UserReviewDto>(updated);
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

        // DELETE: api/UserReview/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var deleted = _userReviewService.Delete(id);
                var dto = _mapper.Map<UserReviewDto>(deleted);
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
