using APIGateway.IdempotencyDb.Entities;

namespace APIGateway.Contracts;

public class LoanPaymentContract
{
    public decimal Amount { get; set; }
    
    public FinancialProfile FinancialProfile { get; set; }
    }