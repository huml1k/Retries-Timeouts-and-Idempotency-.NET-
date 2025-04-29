using APIGateway.IdempotencyDb.Entities;
using APIGateway.Services;

namespace APIGateway.IdempotencyDb.Repositories;

public class IdempotencyRepository : IIdempotencyRepository
{
    private readonly MyDbContext _context;

    public IdempotencyRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ContainsIdempotencyKey(IdempotencyKeyEntity key)
    {
        if (_context.idempotencyKeyEntities.Where(k => k.IdempotencyKey == key.IdempotencyKey).Count() != 0)
        {
            return true;
        }
        return false;
    }

    public async Task AddIdempotencyKey(IdempotencyKeyEntity key)
    {
        
    }
}