namespace APIGateway.IdempotencyDb.Repositories;

public interface IIdempotencyRepository
{
    public async Task CreateIdempotencyKeyAsync(Guid idempotencyKey){}
}