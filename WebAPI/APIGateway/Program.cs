
using APIGateway.IdempotencyDb;
using APIGateway.IdempotencyDb.Repositories;
using APIGateway.Middlewares;
using APIGateway.Routes;
using APIGateway.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<IdempotencyDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(IdempotencyDbContext))));
            

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<RouteManager>();

            builder.Services.AddSingleton<Router>(provider =>
            {
                var routeManager = provider.GetRequiredService<RouteManager>();
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                return new Router(routeManager, httpClientFactory);
            });
            builder.Services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();
            builder.Services.AddScoped<IdempotencyService>();

            var app = builder.Build();
            app.UseMiddleware<IdempotencyMiddleware>();
            app.Run(async (context) =>
            {
                var router = context.RequestServices.GetRequiredService<Router>();
                var content = await router.RouteRequest(context.Request);
                await context.Response.WriteAsync(await content.Content.ReadAsStringAsync());
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
