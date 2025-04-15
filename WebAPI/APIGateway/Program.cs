
using APIGateway.IdempotencyDb;
using APIGateway.IdempotencyDb.Repositories;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(MyDbContext))));
            builder.Services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();
            
            builder.Services.AddScoped<IdempotencyService>();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            
            app.MapControllers();
            
            app.MapGet("/x", () => $"{app.Configuration.GetConnectionString(nameof(MyDbContext)).ToString()}");

            app.Run();
        }
    }
}
