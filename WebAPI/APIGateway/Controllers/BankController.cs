using APIGateway.Contracts;
using APIGateway.IdempotencyDb.Repositories;
using APIGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers;

[Controller]
[Route("api/[controller]")]
public class BankController : Controller
{

    private readonly FinacialProfileRepository _finacialProfileRepository;
    private readonly EmailService _userService;

    public BankController(FinacialProfileRepository financialProfile, EmailService userService)
    {
        _finacialProfileRepository = financialProfile;
        _userService = userService;
    }

    [HttpGet("page")]
    public async Task<IActionResult> GetAuthPage() => View("authPage");

    [HttpPost("pay")]
    public async Task<IActionResult> PayLoan([FromBody] LoanPaymentContract money)
    {
        var userIdFromDb = _userService.GetUserByToken(User).Result.Value;
        var financialProfile = await _finacialProfileRepository.GetFinancialProfile(userIdFromDb);

        if (money.Amount <= financialProfile.Balance && money.Amount > 0)
        {
            await _finacialProfileRepository.WriteOff(financialProfile, money.Amount);
            var a = financialProfile.Balance - money.Amount;
            return Ok(new { newBalance = a });
        }
        return BadRequest();
    }
    
    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] LoanPaymentContract tr)
    {
        var userIdFromDb = _userService.GetUserByToken(User).Result.Value;
        var financialProfileFrom = await _finacialProfileRepository.GetFinancialProfile(userIdFromDb);
        var financialProfileTo = await _finacialProfileRepository.GetFinancialProfile(userIdFromDb);


        if (tr.Amount <= financialProfileFrom.Balance && tr.Amount > 0)
        {
            await _finacialProfileRepository.WriteOff(financialProfileFrom, tr.Amount);
            await _finacialProfileRepository.AddToBalance(financialProfileTo, tr.Amount);
            var newBalance = financialProfileFrom.Balance - tr.Amount;
            return Ok(new { newBalance = newBalance });
        }
        return BadRequest();
    }
}