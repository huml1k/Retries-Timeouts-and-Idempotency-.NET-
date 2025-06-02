using APIGateway.IdempotencyDb.Entities;
using APIGateway.IdempotencyDb.Repositories;
using APIGateway.IdempotencyDb.Repositories.Interfaces;
using APIGateway.Models;
using APIGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Security.Claims;
using Polly.Retry;
using Polly.Timeout;

namespace APIGateway.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class ViewController : Controller
    {

        private readonly EmailService _userService;
        private readonly FinacialProfileRepository _finacialProfileRepository;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly AsyncTimeoutPolicy _timeoutPolicy;

        public ViewController(
            EmailService userService,
            FinacialProfileRepository financialProfile,
            AsyncRetryPolicy retryPolicy,
            AsyncTimeoutPolicy timeoutPolicy)
        {
            _userService = userService;
            _finacialProfileRepository = financialProfile;
            _retryPolicy = retryPolicy;
            _timeoutPolicy = timeoutPolicy;
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetAuthPage() => View("authPage");

        [HttpGet("pageBank")]
        public async Task<IActionResult> GetBankPage(
            [FromQuery] bool simulateTimeout = false,
            [FromQuery] bool simulateError = false)
        {
            var userIdFromDb = _userService.GetUserByToken(User).Result.Value;
            if (string.IsNullOrEmpty(userIdFromDb.ToString()))
                return RedirectToAction("GetAuthPage");
            
            var user = await _userService.GetById(userIdFromDb);

            try
            {
                var financialProfile = await _retryPolicy.ExecuteAsync(async () =>
                    await _timeoutPolicy.ExecuteAsync(async ct =>
                    {
                        if (simulateTimeout) await Task.Delay(7000, ct);
                        if (simulateError) throw new Exception("Simulated error");

                        return await _finacialProfileRepository.GetFinancialProfile(userIdFromDb);
                        ;
                    }, CancellationToken.None));
                var model = new BankViewModel
                {
                    FullName = user.Name,
                    AccountNumber = financialProfile.AccountNumber,
                    Balance = financialProfile.Balance,
                    UnpaidCredit = financialProfile.UnpaidCredit,
                    CreditDueDate = financialProfile.CreditDueDate
                };

                return View("bankSite", model);
            }
            catch (TimeoutException)
            {
                return StatusCode(408, "Operation timed out");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Operation failed: {ex.Message}");
            }
        }
    }
}
