using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly BookLibraryContext _context;
        private readonly LogService _logService;

        public LogController(BookLibraryContext context, LogService logService)
        {
            this._context = context;
            this._logService = logService;
        }

        // GET: api/<LogController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_logService.GetLogs());
        }

        // GET api/<LogController>/5
        [HttpGet("[action]/{n}")]
        public IActionResult Get(int n)
        {
            var logs = _logService.GetLogs()
                .Take(n);

            return Ok(logs);
        }

        [HttpGet("[action]")]
        public IActionResult Count()
        {
            return Ok(_logService.Count());
        }

        // POST api/<LogController>
        [HttpPost]
        public IActionResult Post([FromBody] BookLog bookLog)
        {
            try {
                var log = _logService.Log(bookLog.Message, bookLog.Level);
                var location = Url.Action(nameof(Get), new { id = bookLog.Id });

                return Created(location, log);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
