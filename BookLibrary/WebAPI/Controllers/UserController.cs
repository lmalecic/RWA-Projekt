using AutoMapper;
using DAL.DTO;
using DAL.Models;
using DAL.Security;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BookLibraryContext _context;
        private readonly IMapper _mapper;

        public UserController(IConfiguration configuration, BookLibraryContext context, IMapper mapper)
        {
            this._configuration = configuration;
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Get(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
                NotFound($"User with id {id} not found.");

            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetToken()
        {
            try {
                string? secureKey = _configuration["JWT:SecureKey"];
                if (secureKey == null)
                    throw new Exception("JWT SecureKey is not set in appsettings.json!");

                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 10);

                return Ok(serializedToken);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Register(UserDto userDto)
        {
            try {
                // Check if there is such a username in the database already
                var trimmedUsername = userDto.Username.Trim();
                if (_context.Users.Any(x => x.Username.Equals(trimmedUsername)))
                    return BadRequest($"Username {trimmedUsername} already exists");

                // Create user from DTO and hashed password
                var user = _mapper.Map<User>(userDto);
                user.PwdHash = Argon2.Hash(userDto.Password);

                // Add user and save changes to database
                _context.Add(user);
                _context.SaveChanges();

                // Update DTO Id to return it to the client
                userDto.Id = user.Id;

                var location = Url.Action(nameof(Get), new { id = user.Id });
                return Created(location, userDto);

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Login(UserLoginDto userDto)
        {
            try {
                var genericLoginFail = "Incorrect username or password";

                // Try to get a user from database
                var existingUser = _context.Users.FirstOrDefault(x => x.Username == userDto.Username);
                if (existingUser == null)
                    return BadRequest(genericLoginFail);

                // Check is password hash matches
                if (!Argon2.Verify(existingUser.PwdHash, userDto.Password))
                    return BadRequest(genericLoginFail);

                // Create and return JWT token
                string? secureKey = _configuration["JWT:SecureKey"];
                if (secureKey == null)
                    throw new Exception("JWT SecureKey is not set in appsettings.json!");

                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 120, existingUser.Username, existingUser.Role);

                return Ok(new {
                    token = serializedToken,
                    expiryTime = DateTime.UtcNow.AddMinutes(120)
                });
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username == changePasswordDto.Username);
            if (user == null)
                return NotFound($"User {changePasswordDto.Username} not found.");

            if (!Argon2.Verify(user.PwdHash, changePasswordDto.CurrentPassword))
                return BadRequest("Current password is incorrect.");

            user.PwdHash = Argon2.Hash(changePasswordDto.Password);
            _context.SaveChanges();

            return Ok("Password successfully changed.");
        }
    }
}
