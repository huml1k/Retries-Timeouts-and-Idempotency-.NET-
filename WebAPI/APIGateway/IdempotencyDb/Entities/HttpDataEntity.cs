using System.ComponentModel.DataAnnotations;

namespace APIGateway.IdempotencyDb.Entities
{
    public class HttpDataEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    
        [Required]
        [MaxLength(10)]
        public string RequestMethod { get; set; }
    
        [Required]
        [MaxLength(500)]
        public string RequestPath { get; set; }
    
        public string RequestBody { get; set; }
        public string RequestHeaders { get; set; }
    
        [Required]
        public int ResponseCode { get; set; }
    
        public string ResponseBody { get; set; }
        public string ResponseHeaders { get; set; }
    
        public Guid IdempotencyKeyId { get; set; }
        public IdempotencyKeyEntity IdempotencyKey { get; set; }
    }
}
