using makefriends_web_api.Data;
using makefriends_web_api.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections;

namespace makefriends_web_api.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {

        private static readonly Dictionary<string, User> _users = new();

        [HttpPost("register")]
        public async Task<ActionResult<User>> CreateUser(LoginCredentials credentials)
        {
            SHA512StringFunction func = new SHA512StringFunction();
            byte[] hash = func.GetHash(credentials.Password, out byte[] salt);

            User user = new(credentials.Username, hash, salt);

            _users.Add(user.Username, user);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginCredentials credentials)
        {
            if (_users.TryGetValue(credentials.Username, out User user))
            {
                SHA512StringFunction func = new SHA512StringFunction();

                if (func.Verify(credentials.Password, user.PasswordHash, user.PasswordSalt)) return Ok("Success! <token>");
                return BadRequest("Wrong password");
            }

            return BadRequest("Username not found");
        }


    }
}
