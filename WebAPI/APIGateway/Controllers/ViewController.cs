using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class ViewController : Controller
    {
        [HttpGet("/")]
        public async Task<IActionResult> GetAuthPage() => View("authPage");
    }
}
