using APIGateway.IdempotencyDb.Entities;

namespace APIGateway.IdempotencyDb.Repositories;

public interface IIdempotencyRepository
{
    public Task<bool> ContainsIdempotencyKey(IdempotencyKeyEntity key);
    
    public Task AddIdempotencyKey(IdempotencyKeyEntity key);
}