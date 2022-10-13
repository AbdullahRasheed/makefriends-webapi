using makefriends_web_api.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace makefriends_web_api.Controllers
{
    [ApiController]
    [Route("avatar")]
    public class AvatarController : ControllerBase
    {

        private readonly AvatarService _avatarService;

        public AvatarController(AvatarService avatarService) => _avatarService = avatarService;

        [HttpPost("upload")]
        public async Task<ActionResult> Upload([FromForm] IFormFile file)
        {
            if (file is null) return BadRequest("File null");

            await _avatarService.InsertAvatar(file, "test");

            return Ok("uploaded");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<byte[]>> Download(string id)
        {
            return await _avatarService.FindAvatar(ObjectId.Parse(id));
        }
    }
}
