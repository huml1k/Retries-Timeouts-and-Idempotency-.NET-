namespace APIGateway.IdempotencyDb.Entities
{
    public class IdempotencyKeyEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LockedAt { get; set; }
        public string IdempotencyKey { get; set; }
        public Guid HttpExchanceDataID { get; set; }
        public HttpDataEntity HttpDataEntity { get; set; }
    }
}
