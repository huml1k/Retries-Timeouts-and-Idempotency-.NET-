using APIGateway.IdempotencyDb.Entities;
using APIGateway.IdempotencyDb.Repositories.interfaces;
using APIGateway.Services;
using Microsoft.EntityFrameworkCore;

namespace APIGateway.IdempotencyDb.Repositories;

public class IdempotencyRepository : IIdempotencyRepository
{
    private readonly IdempotencyDbContext _context;

    public IdempotencyRepository(IdempotencyDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ContainsIdempotencyKey(IdempotencyKeyEntity key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (string.IsNullOrWhiteSpace(key.IdempotencyKey)) 
            throw new ArgumentException("Idempotency key cannot be null or empty");

        return await _context.idempotencyKeyEntities
            .AsNoTracking()
            .AnyAsync(k => k.IdempotencyKey == key.IdempotencyKey)
            .ConfigureAwait(false); 
    }

    public async Task AddIdempotencyKey(IdempotencyKeyEntity key)
    {
        
    }
}