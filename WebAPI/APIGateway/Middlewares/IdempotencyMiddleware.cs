namespace APIGateway.Middlewares;

public class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
 
    public IdempotencyMiddleware(RequestDelegate next)
    {
        this._next = next;
    }
 
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("Ideem", "deeeem");
        await _next(context);
    }
}