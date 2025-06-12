using AutoMapper;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebAPI.DTO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BookLocationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAssociationService<BookLocation> _bookLocationService;
        private readonly LogService _logService;

        public BookLocationController(IAssociationService<BookLocation> bookLocationService, LogService logService, IMapper mapper)
        {
            _bookLocationService = bookLocationService;
            _logService = logService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _bookLocationService.GetAll()
                .Select(_mapper.Map<BookLocationDto>);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromQuery] BookLocationDto entityDto)
        {
            try {
                var dbEntity = _mapper.Map<BookLocation>(entityDto);
                var result = _bookLocationService.Create(dbEntity);
                var mapped = _mapper.Map<BookLocationDto>(result);
                return Ok(mapped);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while associating bookId {entityDto.BookId} with locationId {entityDto.LocationId}.", 2);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] BookLocationDto entityDto)
        {
            try {
                var dbEntity = _mapper.Map<BookLocation>(entityDto);
                var result = _bookLocationService.Delete(dbEntity);
                var mapped = _mapper.Map<BookLocationDto>(result);
                return Ok(mapped);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while de-associating bookId {entityDto.BookId} with locationId {entityDto.LocationId}.", 2);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
