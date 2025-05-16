namespace APIGateway.Contracts
{
    public record UserContract
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
