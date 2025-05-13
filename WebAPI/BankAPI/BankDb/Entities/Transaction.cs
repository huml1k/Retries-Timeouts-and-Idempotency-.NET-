using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.BankDb.Entities;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string FromAccountNumber { get; set; }

    [StringLength(20)]
    public string ToAccountNumber { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(3)]
    public string Currency { get; set; }

    [Required]
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(50)]
    public string TransactionType { get; set; } // "Deposit", "Withdrawal", "Transfer"

    [StringLength(500)]
    public string Description { get; set; }

    // Навигационные свойства
    [ForeignKey("FromAccountNumber")]
    public Account FromAccount { get; set; }

    [ForeignKey("ToAccountNumber")]
    public Account ToAccount { get; set; }
}