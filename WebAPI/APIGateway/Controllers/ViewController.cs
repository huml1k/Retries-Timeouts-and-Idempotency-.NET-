using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla;

namespace APIGateway.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class ViewController : Controller
    {
        [HttpGet("page")]
        public async Task<IActionResult> GetAuthPage() => View("authPage");

        [HttpGet("pageBank")]
        public async Task<IActionResult> GetBankPage() => View("bankSite");
    }
}
