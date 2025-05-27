using APIGateway.IdempotencyDb.Entities;
using System.Text;

namespace APIGateway.IdempotencyDb.Entities
{
    public class FinancialProfile
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal UnpaidCredit { get; set; }
        public DateTime CreditDueDate { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; }

        public static FinancialProfile CreateRandom(Guid userId)
        {
            return new FinancialProfile
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AccountNumber = GenerateAccountNumber(),
                Balance = GenerateRandomBalance(),
                UnpaidCredit = GenerateRandomCredit(),
                CreditDueDate = GenerateRandomDueDate()
            };
        }

        // Генерация номера счета по стандарту IBAN
        public  static string GenerateAccountNumber()
        {
            var rand = new Random();
            var sb = new StringBuilder("RU"); // Страна-код

            // Контрольные цифры
            sb.Append(rand.Next(10, 99).ToString("D2"));

            // БИК банка
            sb.Append("04"); // Пример кода банка

            // 20-значный номер счета
            for (int i = 0; i < 20; i++)
            {
                sb.Append(rand.Next(0, 9));
            }

            return sb.ToString();
        }

        // Генерация баланса (от 0 до 1 млн)
        private static decimal GenerateRandomBalance()
        {
            var rand = new Random();
            return Math.Round((decimal)rand.NextDouble() * 50_000, 2);
        }

        // Генерация кредита (от 10 тыс до 5 млн)
        private static decimal GenerateRandomCredit()
        {
            var rand = new Random();
            return Math.Round((decimal)rand.NextDouble() * 5_000_000 + 10_000, 2);
        }

        // Генерация даты погашения (от 6 месяцев до 5 лет)
        private static DateTime GenerateRandomDueDate()
        {
            var rand = new Random();
            return DateTime.UtcNow.AddMonths(rand.Next(6, 60));
        }
    }
}

public static class FinancialProfileExtensions
{
    private static readonly Random _random = new Random();
    private static readonly object _syncLock = new object();


    public static string GenerateBankAccountNumber(this object source, string bankCode = "04")
    {
        var sb = new StringBuilder("RU"); // Страна-код

        // Контрольные цифры (от 10 до 99)
        sb.Append(_random.Next(10, 99).ToString("D2"));

        // Код банка
        sb.Append(bankCode);

        // 20-значный номер счета
        for (int i = 0; i < 20; i++)
        {
            sb.Append(_random.Next(0, 9));
        }

        return sb.ToString();
    }
    public static decimal GenerateRandomBalance(this FinancialProfile profile)
    {
        lock (_syncLock)
        {
            return Math.Round((decimal)_random.NextDouble() * 50_000, 2);
        }
    }

    public static decimal GenerateRandomCredit(this FinancialProfile profile)
    {
        lock (_syncLock)
        {
            return Math.Round((decimal)_random.NextDouble() * 5_000_000 + 10_000, 2);
        }
    }

    public static DateTime GenerateRandomDueDate(this FinancialProfile profile)
    {
        lock (_syncLock)
        {
            return DateTime.UtcNow.AddMonths(_random.Next(6, 60));
        }
    }
}
