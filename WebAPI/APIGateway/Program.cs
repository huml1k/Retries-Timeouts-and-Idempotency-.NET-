
<<<<<<< Updated upstream
=======
using APIGateway.IdempotencyDb;
using APIGateway.IdempotencyDb.Repositories;
using APIGateway.IdempotencyDb.Repositories.Interfaces;
using APIGateway.Middlewares;
>>>>>>> Stashed changes
using APIGateway.Routes;
using Microsoft.Extensions.DependencyInjection;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<RouteManager>();

            builder.Services.AddSingleton<Router>(provider =>
            {
                var routeManager = provider.GetRequiredService<RouteManager>();
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                return new Router(routeManager, httpClientFactory);
            });
<<<<<<< Updated upstream

            var app = builder.Build();
=======

            builder.Services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();
            builder.Services.AddScoped<IdempotencyService>();

            var app = builder.Build();
            app.UseMiddleware<IdempotencyMiddleware>();
>>>>>>> Stashed changes

            app.Run(async (context) =>
            {
                var router = context.RequestServices.GetRequiredService<Router>();
                var content = await router.RouteRequest(context.Request);
                await context.Response.WriteAsync(await content.Content.ReadAsStringAsync());
            });

<<<<<<< Updated upstream
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

=======
            app.UseSwagger();
            app.UseSwaggerUI();
    
            app.UseHttpsRedirection();
            app.UseStaticFiles();
>>>>>>> Stashed changes
            app.UseAuthorization();


            app.MapControllers();

            app.Run("http://0.0.0.0:80");
        }
    }
}
