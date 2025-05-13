using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.BankDb.Entities;

public class Account
{
    [Key]
    [StringLength(20)]
    public string AccountNumber { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }

    [Required]
    [StringLength(50)]
    public string Currency { get; set; } = "RUB";

    [Required]
    public DateTime OpenedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ClosedDate { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    // Навигационные свойства
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}