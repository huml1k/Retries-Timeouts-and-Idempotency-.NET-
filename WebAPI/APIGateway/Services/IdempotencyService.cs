using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using APIGateway.IdempotencyDb;
using APIGateway.IdempotencyDb.Entities;
using APIGateway.IdempotencyDb.Repositories;
using Newtonsoft.Json;

namespace APIGateway.Services;

public class IdempotencyService 
{
    private readonly IIdempotencyRepository _idempotencyRepository;

    public IdempotencyService(IIdempotencyRepository repository)
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

    public async Task<string> GenerateIdempotencyKey(string userId, object requestBody)  // конкат userId и requestBody и получить хэш
    {
        // var sha = SHA256.Create();
        // var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(userId + json));
        // return Convert.ToHexString(hashBytes);
        return Guid.NewGuid().ToString();
    }
}