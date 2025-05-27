using APIGateway.Contracts;
using APIGateway.IdempotencyDb.Entities;
using APIGateway.Infrastructure;
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
        private readonly PasswordHasher _passwordHasher;

        public AuthController(EmailService emailService, PasswordHasher passwordHasher)
        {
            _emailService = emailService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegistretionUser([FromBody] RegisterContract registerInfo)
        {
            var fin = new FinancialProfile();
            var id = new Guid();
            var finId = new Guid();
            var userEntity = new UserEntity
            {
                Id = id,
                Name = registerInfo.Name,
                Password = _passwordHasher.GenerateTokenSHA(registerInfo.Password),
                Email = registerInfo.Email,
                FinancialProfile = new FinancialProfile
                {
                    Id = new Guid(),
                    UserId = id,
                    AccountNumber = fin.GenerateBankAccountNumber(),
                    Balance = fin.GenerateRandomBalance(),
                    UnpaidCredit = fin.GenerateRandomCredit(),
                    CreditDueDate = fin.GenerateRandomDueDate()
                },
            };
            await _emailService.Register(userEntity);

            return Ok("Регистрация прошла успешно!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserContract userEntity)
        {
            var token = await _emailService.Login(userEntity.Email, userEntity.Password);
            Response.Cookies.Append("token-user", token);

            return Ok(new {token});
        }
    }
}
