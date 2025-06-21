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

       
    


    }
}
