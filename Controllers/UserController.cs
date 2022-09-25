using makefriends_web_api.Data;
using makefriends_web_api.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections;
using makefriends_web_api.Database;

namespace makefriends_web_api.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {

        private readonly UserService _userService;

        public UserController(UserService userService) => _userService = userService;

        [HttpPost("register")]
        public async Task<ActionResult<User>> CreateUser([FromBody] LoginCredentials credentials)
        {
            SHA512StringFunction func = new SHA512StringFunction();
            byte[] hash = func.GetHash(credentials.Password, out byte[] salt);

            var response = await _userService.FindByUsernameAsync(credentials.Username);

            if (response is not null)
            {
                return BadRequest("Username already exists");
            }

            var user = new User(credentials.Username, hash, salt);

            await _userService.InsertAsync(user);

            return Ok("Successfully created account");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginCredentials credentials)
        {
            var user = await _userService.FindByUsernameAsync(credentials.Username);

            if (user is not null)
            {
                SHA512StringFunction func = new SHA512StringFunction();

                if (func.Verify(credentials.Password, user.PasswordHash, user.PasswordSalt)) return Ok("Success! <token>");
            }

            return BadRequest("Incorrect username or password");
        }


    }
}
