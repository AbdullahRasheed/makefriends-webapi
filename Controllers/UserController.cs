using makefriends_web_api.Data;
using makefriends_web_api.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections;
using makefriends_web_api.Database;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace makefriends_web_api.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {

        private readonly UserService _userService;

        public UserController(UserService userService) => _userService = userService;

        [HttpPost("register")]
        public async Task<ActionResult> CreateUser([FromBody] LoginCredentials credentials)
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

            // TODO After moving all the code from this class into separate classes,
            // login the user after creating their account

            return Ok("Successfully created account");
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginCredentials credentials)
        {
            var user = await _userService.FindByUsernameAsync(credentials.Username);

            if (user is not null)
            {
                SHA512StringFunction func = new SHA512StringFunction();

                if (func.Verify(credentials.Password, user.PasswordHash, user.PasswordSalt))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

                    return Ok();
                }
            }

            return BadRequest("Incorrect username or password");
        }
    }
}
