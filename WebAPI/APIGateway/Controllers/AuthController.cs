using APIGateway.IdempotencyDb.Entities;
using APIGateway.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace APIGateway.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly EmailService _emailService;

        public AuthController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegistretionUser([FromBody] UserEntity userEntity) 
        {
            await _emailService.Register(userEntity);

            return Ok("Регистрация прошла успешно!");
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody] UserEntity userEntity) 
        {
            var token = await _emailService.Login(userEntity);
            Response.Cookies.Append("token-user", token);

            return Redirect("http://bankapi/BankOperation/TestingPost");
        }
    }
}
