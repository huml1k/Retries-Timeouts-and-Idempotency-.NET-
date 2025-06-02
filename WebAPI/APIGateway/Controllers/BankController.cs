using APIGateway.Contracts;
using APIGateway.IdempotencyDb.Repositories;
using APIGateway.Services;
using Microsoft.AspNetCore.Mvc;
using Polly.Retry;
using Polly.Timeout;

namespace APIGateway.Controllers;

[Controller]
[Route("api/[controller]")]
public class BankController : Controller
{

    private readonly FinacialProfileRepository _finacialProfileRepository;
    private readonly EmailService _userService;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly AsyncTimeoutPolicy _timeoutPolicy; 

    public BankController(
        FinacialProfileRepository financialProfile,
        EmailService userService,
        AsyncRetryPolicy retryPolicy,
        AsyncTimeoutPolicy timeoutPolicy)
    {
        _finacialProfileRepository = financialProfile;
        _userService = userService;
        _retryPolicy = retryPolicy;
        _timeoutPolicy = timeoutPolicy;
    }

    [HttpGet("page")]
    public async Task<IActionResult> GetAuthPage() => View("authPage");

    [HttpPost("pay")]
    public async Task<IActionResult> PayLoan(
        [FromBody] LoanPaymentContract money,
        [FromQuery] bool simulateTimeout = false,
        [FromQuery] bool simulateError = false)
    {
        try
        {
            var userIdFromDb = _userService.GetUserByToken(User).Result.Value;
            var financialProfile = await _retryPolicy.ExecuteAsync(async x =>
                {
                    if (simulateTimeout)
                    {
                        await Task.Delay(7000, x);
                    }

                    if (simulateError)
                    {
                        throw new Exception("Simulated error");   
                    }
                    return await _finacialProfileRepository.GetFinancialProfile(userIdFromDb);
                }, CancellationToken.None);
                

            if (money.Amount <= financialProfile.Balance && money.Amount > 0)
            {
                await _finacialProfileRepository.WriteOff(financialProfile, money.Amount);
                var a = financialProfile.Balance - money.Amount;
                return Ok(new { newBalance = a });
            }

            return BadRequest();
        }
        catch (TimeoutRejectedException)
        {
            return StatusCode(408, "Operation timed out");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Operation failed: {ex.Message}"); 
        }
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