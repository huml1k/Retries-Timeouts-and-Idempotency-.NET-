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
            return Ok("Hello pidor");
        }

        [HttpGet("health")]
        public IActionResult HealthCheck() => Ok();
    }
}