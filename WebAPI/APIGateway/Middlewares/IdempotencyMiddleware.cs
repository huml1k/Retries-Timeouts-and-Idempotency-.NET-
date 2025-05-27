using APIGateway.Services;
using APIGateway.IdempotencyDb.Repositories.interfaces;
using APIGateway.IdempotencyDb.Entities;
using System.Text;

namespace APIGateway.Middlewares;

public class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IdempotencyMiddleware> _logger;

    public IdempotencyMiddleware(
            RequestDelegate next,
            ILogger<IdempotencyMiddleware> logger
           )
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IIdempotencyRepository idempotencyRepository, IIdempotencyService _idempotencyService)
    {        // Пропускаем запросы, которые не являются POST
        if (context.Request.Method != HttpMethods.Post)
        {
            await _next(context);
            return;
        }

        // Проверяем, есть ли уже ключ идемпотентности в заголовках
        if (!context.Request.Headers.ContainsKey("Idempotency-Key"))
        {


            var authCookie = context.Request.Cookies.TryGetValue("token-user", out var cookieValue).ToString();
            var idempotencyKey = await _idempotencyService.GenerateIdempotencyKey(authCookie, context.Request);
            // Сохраняем ключ в хранилище
            var keyEntity = new IdempotencyKeyEntity
            {
                IdempotencyKey = idempotencyKey,
                CreateDate = DateTime.UtcNow,
                HttpDataEntity = new HttpDataEntity
                {
                    RequestMethod = HttpMethods.Post,
                    RequestPath = context.Request.Path,
                    ResponseCode = 200,
                }
               
            };

            try
            {
                await idempotencyRepository.AddIdempotencyKey(keyEntity);
             
            }
            catch (Exception ex)
            {
               
                // Можно продолжить выполнение, даже если не удалось сохранить ключ
            }
        }
        else
        {
            var existingKey = context.Request.Headers["Idempotency-Key"].ToString();
          

            // Проверяем, есть ли такой ключ в хранилище
            var keyEntity = new IdempotencyKeyEntity { IdempotencyKey = existingKey };
            var keyExists = await idempotencyRepository.ContainsIdempotencyKey(keyEntity);

            if (keyExists)
            {
               
                // Можно добавить логику обработки повторного запроса
            }
        }

        // Добавляем ключ в ответ для клиента
        context.Response.OnStarting(() =>
        {
            if (context.Request.Headers.TryGetValue("Idempotency-Key", out var key))
            {
                context.Response.Headers.Add("Idempotency-Key", key);
            }
            return Task.CompletedTask;
        });

        await _next(context);
    }


}

public static class IdempotencyMiddlewareExtensions
{
    public static IApplicationBuilder UseIdempotencyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IdempotencyMiddleware>();
    }
}