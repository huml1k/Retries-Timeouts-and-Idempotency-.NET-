using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class ViewController : Controller
    {
        [HttpGet("page")]
        public async Task<IActionResult> GetAuthPage() => View("authPage");
    }
}
