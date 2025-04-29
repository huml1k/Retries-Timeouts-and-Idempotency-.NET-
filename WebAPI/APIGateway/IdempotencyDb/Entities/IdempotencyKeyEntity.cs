using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGateway.IdempotencyDb.Entities
{
    public class IdempotencyKeyEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime? LockedAt { get; set; }
    
        [Required]
        [MaxLength(256)]
        public string IdempotencyKey { get; set; }
    
        [ForeignKey(nameof(HttpDataEntity))]
        public Guid HttpExchanceDataID { get; set; }
    
        public HttpDataEntity HttpDataEntity { get; set; }
    }
}
