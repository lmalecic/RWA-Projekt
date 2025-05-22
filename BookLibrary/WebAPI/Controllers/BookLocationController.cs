using AutoMapper;
using DAL.DTO;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
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
        public IActionResult Post(int id1, int id2)
        {
            try {
                var result = _bookLocationService.Create(id1, id2);
                var mapped = _mapper.Map<BookLocationDto>(result);
                return Ok(mapped);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while associating bookId {id1} with locationId {id2}.", 2);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id1, int id2)
        {
            try {
                var result = _bookLocationService.Delete(id1, id2);
                var mapped = _mapper.Map<BookLocationDto>(result);
                return Ok(mapped);
            }
            catch (Exception ex) {
                _logService.Log($"An error has occurred while de-associating bookId {id1} with locationId {id2}.", 2);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
