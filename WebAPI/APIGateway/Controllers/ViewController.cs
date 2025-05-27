using APIGateway.IdempotencyDb.Entities;
using APIGateway.IdempotencyDb.Repositories;
using APIGateway.IdempotencyDb.Repositories.Interfaces;
using APIGateway.Models;
using APIGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Security.Claims;

namespace APIGateway.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class ViewController : Controller
    {

        private readonly EmailService _userService;
        private readonly FinacialProfileRepository _finacialProfileRepository;

        public ViewController(EmailService userService)
        {
            _userService = userService;
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetAuthPage() => View("authPage");

        [HttpGet("pageBank")]
        public async Task<IActionResult> GetBankPage()
        {
            var userIdFromDb = _userService.GetUserByToken(User).Result.Value;

            if (string.IsNullOrEmpty(userIdFromDb.ToString()))
                return RedirectToAction("GetAuthPage");

            var user = await _userService.GetById(userIdFromDb);
            var userId = user.Id;

            // Получаем финансовый профиль
            var financialProfile = await _finacialProfileRepository.GetFinancialProfile(userId);
               

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
    }
}
