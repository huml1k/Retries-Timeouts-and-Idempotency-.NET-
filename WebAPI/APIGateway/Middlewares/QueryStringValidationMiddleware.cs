namespace APIGateway.Middlewares
{
    public class QueryStringValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public QueryStringValidationMiddleware(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api")) 
            {
                await _next(context);
                return;
            }
            await _next(context);
        }
    }
}
