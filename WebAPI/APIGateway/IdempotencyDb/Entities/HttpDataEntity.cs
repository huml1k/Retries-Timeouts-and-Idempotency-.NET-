namespace APIGateway.IdempotencyDb.Entities
{
    public class HttpDataEntity
    {
        public Guid Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestMethod { get; set; }
        public string RequestPath { get; set; }
        public string RequestBody { get; set; }
        public string RequestHeaders { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseBody { get; set; }
        public string ResponseHeaders { get; set; }
        public Guid IdempotencyKeyId { get; set; }
        public IdempotencyKeyEntity IdempotencyKey { get; set; }
    }
}
