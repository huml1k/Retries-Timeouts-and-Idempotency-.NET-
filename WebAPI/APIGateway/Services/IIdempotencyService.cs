namespace APIGateway.Services;

public interface IIdempotencyService
{
    public Task<bool> IsIdempotent(HttpRequest request);
    
    public Task AddIdempotencyKey(HttpRequest request);

   
    public Task<string> GenerateIdempotencyKey(string userId, HttpRequest request);
}