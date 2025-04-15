using APIGateway.IdempotencyDb.Entities;

namespace APIGateway.IdempotencyDb.Repositories;

public class IdempotencyRepository : IIdempotencyRepository
{
    private readonly MyDbContext _context;

    public IdempotencyRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task CreateIdempotencyKeyAsync(Guid idempotencyKey)
    {
        var idempotencyKeyEntity = new IdempotencyKeyEntity()
        {
            Id = Guid.NewGuid(),
            IdempotencyKey = idempotencyKey,
            CreateDate = DateTime.UtcNow
        };

        _context.idempotencyKeyEntities.Add(idempotencyKeyEntity);
    }
}