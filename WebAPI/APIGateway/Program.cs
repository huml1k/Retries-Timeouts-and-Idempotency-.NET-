using APIGateway.AuthCheck;
using APIGateway.IdempotencyDb;
using APIGateway.IdempotencyDb.Repositories;
using APIGateway.IdempotencyDb.Repositories.interfaces;
using APIGateway.IdempotencyDb.Repositories.Interfaces;
using APIGateway.Infrastructure;
using APIGateway.Middlewares;
using APIGateway.Routes;
using APIGateway.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace APIGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Конфигурация базы данных
            builder.Services.Configure<JwtOption>(builder.Configuration.GetSection(nameof(JwtOption)));
            builder.Services.AddDbContext<IdempotencyDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("MyDbContext")));

            // Регистрация сервисов
            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();

            // Singleton сервисы
            builder.Services.AddSingleton<RouteManager>();
            builder.Services.AddSingleton<Router>(provider =>
            {
                var routeManager = provider.GetRequiredService<RouteManager>();
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                return new Router(routeManager, httpClientFactory);
            });



            // Scoped сервисы
            builder.Services.AddScoped<JwtOption>();
            builder.Services.AddScoped<JwtProvider>();
            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();
            builder.Services.AddScoped<IIdempotencyService, IdempotencyService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<FinacialProfileRepository>();
            builder.Services.AddAuthOption(builder.Configuration);


            // Transient сервисы

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseCors("AllowAll");
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Применение миграций базы данных
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<IdempotencyDbContext>();
                await dbContext.Database.MigrateAsync();
            }

            // Конфигурация middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseIdempotencyMiddleware();
            

            // Добавление middleware идемпотентности
           

            app.MapControllers();

            app.MapFallback(async (context) =>
            {
                var router = context.RequestServices.GetRequiredService<Router>();
                var content = await router.RouteRequest(context.Request);
                await context.Response.WriteAsync(await content.Content.ReadAsStringAsync());
            });

            app.Run();
        }
    }
}