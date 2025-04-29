using APIGateway.IdempotencyDb;
using APIGateway.IdempotencyDb.Entities;
using APIGateway.IdempotencyDb.Repositories;

namespace APIGateway.Services;

public class IdempotencyService : IIdempotencyService
{
    private readonly IdempotencyRepository _idempotencyRepository;

    public IdempotencyService(IdempotencyRepository repository)
    {
        _idempotencyRepository = repository;
    }
    
    public async Task<bool> IsIdempotent(HttpRequest request)
    {
        var idempotencyKey = Guid.Parse(request.Headers["Idempotency-Key"].ToString());
        var key = new IdempotencyKeyEntity()
        {
            IdempotencyKey = idempotencyKey
        };
        if (await _idempotencyRepository.ContainsIdempotencyKey(key))
        {
            return true;
        }
        return false;
    }

    public async Task AddIdempotencyKey(HttpRequest request)
    {
        
    }
}