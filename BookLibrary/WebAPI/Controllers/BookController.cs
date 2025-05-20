using DAL.Models;
using DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookController : ControllerBase
	{
		private readonly BookLibraryContext _context;
		private readonly IMapper _mapper;

		public BookController(BookLibraryContext _context, IMapper _mapper)
		{
			this._context = _context;
			this._mapper = _mapper;
        }

		// GET: api/<BookController>
		[HttpGet]
		public IActionResult Get()
		{
			try {
				var result = _context.Books;
				var mappedResult = _mapper.Map<IEnumerable<BookDto>>(result);

				return Ok(mappedResult);
            }
			catch (Exception ex) {
				return StatusCode(500, ex.Message);
			}
		}

		// GET api/<BookController>/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			try {
				var result = _context.Books
					.FirstOrDefault(x => x.Id == id);
				var mappedResult = _mapper.Map<BookDto>(result);

                return Ok(mappedResult);
			}
			catch (Exception ex) {
				return StatusCode(500, ex.Message);
			}
		}

		// POST api/<BookController>
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public void Post([FromBody] string value)
		{

		}

		// PUT api/<BookController>/5
		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{

		}

		// DELETE api/<BookController>/5
		[Authorize(Roles = "Admin")]
		[HttpDelete("{id}")]
		public void Delete(int id)
		{

		}
	}
}
