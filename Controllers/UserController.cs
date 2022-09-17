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
        public async Task<ActionResult<User>> CreateUser(LoginCredentials credentials)
        {
            SHA512StringFunction func = new SHA512StringFunction();
            byte[] hash = func.GetHash(credentials.Password, out byte[] salt);

            var user = new User(credentials.Username, hash, salt);

            await _userService.InsertAsync(user);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginCredentials credentials)
        {
            var user = await _userService.FindByUsernameAsync(credentials.Username);

            if (user is not null)
            {
                SHA512StringFunction func = new SHA512StringFunction();

                if (func.Verify(credentials.Password, user.PasswordHash, user.PasswordSalt)) return Ok("Success! <token>");
                return BadRequest("Wrong password");
            }

            return BadRequest("Username not found");
        }


    }
}
