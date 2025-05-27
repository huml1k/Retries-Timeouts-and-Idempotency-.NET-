namespace APIGateway.Models
{
    public class BankViewModel
    {
        public string FullName { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal UnpaidCredit { get; set; }
        public DateTime CreditDueDate { get; set; }
    }
}