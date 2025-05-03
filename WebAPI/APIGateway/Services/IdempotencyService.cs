using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using APIGateway.IdempotencyDb;
using APIGateway.IdempotencyDb.Entities;
using APIGateway.IdempotencyDb.Repositories.Interfaces;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        var idempotencyKey = request.Headers["Idempotency-Key"].ToString();
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
        _idempotencyRepository.AddIdempotencyKey(new IdempotencyKeyEntity());
    }

    public async Task<string> GenerateIdempotencyKey(string userId, object requestBody)
    {
        var json = JsonSerializer.Serialize(requestBody);
        var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userId + json));
        return Convert.ToHexString(hashBytes);
    }
}