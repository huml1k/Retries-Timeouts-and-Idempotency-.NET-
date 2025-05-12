namespace APIGateway.Infrastructure
{
    public class JwtOption
    {
        public string SercretKey { get; set; } = string.Empty;

        public int ExpiresHours { get; set; }
    }
}
