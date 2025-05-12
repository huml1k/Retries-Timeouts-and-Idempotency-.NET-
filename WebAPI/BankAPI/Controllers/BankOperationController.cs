using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankOperationController : ControllerBase
    {
        [HttpGet("Testing")]
        public IActionResult GetResult()
        {
            return Ok("Privet Emirka");
        }

        [HttpGet("health")]
        public IActionResult HealthCheck() => Ok();
        
        [HttpPost("TestingPost")]
        public IActionResult PayToUser([FromBody] string userId, float amount)
        {
            return Ok(Response.Headers.ToString());
        }
    }
}