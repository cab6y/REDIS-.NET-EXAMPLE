using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REDIS_.NET_API.services;
using StackExchange.Redis;

namespace REDIS_.NET_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisManagementController : ControllerBase
    {
        private readonly RedisManagementService _redisManagementService;
        public RedisManagementController(RedisManagementService redisManagementService)
        {
            _redisManagementService = redisManagementService;
        }
        [HttpPost("InsertValues")]
        public async Task<IActionResult> InsertValues(List<Dictionary<string, string>> values)
        {
            var result = await _redisManagementService.InsertValues(values);
            if (result == true)
                return Ok("Values inserted successfully.");
            else
                return BadRequest("Failed to insert values.");
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllValues()
        {
            var getlist = await _redisManagementService.GetListAsync();

            return Ok(getlist);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterValues([FromQuery] string? key)
        {
           var filtered = await _redisManagementService.GetByKeyFilter(key);

            return Ok(filtered);
        }

        [HttpPost("PostUserAge")]
        public async Task<IActionResult> PostUserAges(List<Dictionary<string,int>> userages)
        {
            var result = await _redisManagementService.PostUserAges(userages);
            if (result == true)
                return Ok("User ages inserted successfully.");
            else
                return BadRequest("Failed to insert user ages.");
        }

        [HttpGet("GetUserAge")]
        public async Task<IActionResult> GetUserAge(string key)
        {
            var userAge = await _redisManagementService.GetUserAge(key);
            if (userAge != null)
                return Ok(userAge);
            else
                return NotFound("User age not found.");
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(string field,string value)
        {
            var result = await _redisManagementService.SendMessage(field, value);
            if (result == true)
                return Ok("Message sent successfully.");
            else
                return BadRequest("Failed to send message.");
        }
    }
}
