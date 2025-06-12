using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;

        public LogController(LogService logService)
        {
            this._logService = logService;
        }

        // GET: api/<LogController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_logService.GetLogs());
        }

        // GET api/<LogController>/5
        [HttpGet("{n}")]
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
    }
}
