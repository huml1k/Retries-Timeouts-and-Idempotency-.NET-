using APIGateway.Contracts;
using APIGateway.IdempotencyDb.Entities;
using APIGateway.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserContract userEntity)
        {
            var token = await _emailService.Login(userEntity.Email, userEntity.Password);
            Response.Cookies.Append("token-user", token);

            return Redirect("http://bankapi/BankOperation/TestingPost");
        }
    }
}
