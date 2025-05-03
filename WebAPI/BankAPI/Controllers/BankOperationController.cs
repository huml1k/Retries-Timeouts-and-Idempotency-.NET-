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
<<<<<<< Updated upstream
            return Ok("Hello pidor");
        }

        [HttpGet("health")]
        public IActionResult HealthCheck() => Ok();
=======
            return Ok("Hello emirka");
        }
        
        [HttpPost("TestingPost")]
        public IActionResult PayToUser([FromBody] string userId, float amount)
        {
            return Ok(Response.Headers.ToString());
        }
>>>>>>> Stashed changes
    }
}