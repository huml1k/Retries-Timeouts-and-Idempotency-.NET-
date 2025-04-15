using APIGateway.IdempotencyDb;
using APIGateway.IdempotencyDb.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IdempotencyRepository _idempotencyRepository;
    
    public TestController(IdempotencyRepository idempotencyRepository)
    {
        _idempotencyRepository = idempotencyRepository;
    }
    
    [HttpGet("AddKey")]
    public async Task Get()
    {
        await _idempotencyRepository.CreateIdempotencyKeyAsync(Guid.NewGuid());
    }
}