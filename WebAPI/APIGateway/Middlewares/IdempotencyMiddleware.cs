using APIGateway.Services;

namespace APIGateway.Middlewares;

public class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    // private readonly IdempotencyService _idempotencyService;
 
    public IdempotencyMiddleware(RequestDelegate next)
    {
        this._next = next;
    }
 
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("KeyCheck", "qwe");
        if(context.Request.Method == "POST")
        {
            if(!context.Request.Headers.ContainsKey("Idempotency-Key"))
            {
                context.Response.Headers.Add("Idempotency-Key", "" + Guid.NewGuid().ToString());
            }
        }
        await _next(context);
    }
}