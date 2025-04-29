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
            return Ok("Привет пидор");
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