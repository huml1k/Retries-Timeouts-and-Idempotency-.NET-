using BankAPI.BankDb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankOperationController : ControllerBase
    {
        private readonly AccountRepository _accountRepository;
        private readonly TransactionRepository _transactionRepository;
        private readonly CustomerRepository _customerRepository;
        
        public BankOperationController(AccountRepository accountRepository, TransactionRepository transactionRepository,
            CustomerRepository customerRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _customerRepository = customerRepository;
        }
        
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
        
        [HttpPut("TestingPut")]
        public IActionResult AddToBalance([FromBody] string userId, float amount)
        {
            return Ok();
        }
        
        [HttpDelete("TestingDelete")]
        public IActionResult RemoveFromBalance([FromBody] string userId, float amount)
        {
            return Ok();
        }
    }
}