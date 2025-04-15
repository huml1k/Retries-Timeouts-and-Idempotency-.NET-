using APIGateway.IdempotencyDb.Repositories;

namespace APIGateway;

public class IdempotencyService
{
    private readonly IdempotencyRepository _idempotencyRepository;
    
    public IdempotencyService(IdempotencyRepository idempotencyRepository)
    {
        _idempotencyRepository = idempotencyRepository;
    }
    
    public async Task CreateIdempotencyKeyAsync(Guid idempotencyKey)
    {
        await _idempotencyRepository.CreateIdempotencyKeyAsync(idempotencyKey);
    }
}