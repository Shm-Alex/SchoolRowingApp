using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using SchoolRowingApp.Infrastructure.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        {
            // Автоматически преобразует имена свойств в camelCase
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            // Дополнительные настройки (опционально)
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

// Регистрируем MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(SchoolRowingApp.Application.Athletes.Commands.CreateAthleteCommand).Assembly));

var app = builder.Build();
// Применение миграций при запуске приложения
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        // Проверяем, существует ли таблица миграций
        if (!context.Database.GetPendingMigrations().Any())
        {
            Console.WriteLine("Миграции уже применены. Пропускаем...");
            return;
        }

        context.Database.Migrate();
        Console.WriteLine("Миграции успешно применены.");
    }
    catch (Exception ex) when (ex is NpgsqlException npgEx && npgEx.SqlState == "42P07")
    {
        // Таблица _EFMigrationsHistory уже существует — пропускаем
        Console.WriteLine("Таблица миграций уже существует. Пропускаем...");
    }
    catch (Exception ex)
    {
        // Логируем другие ошибки
        Console.WriteLine($"Ошибка миграции: {ex.Message}");
        throw;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();  // ← вместо MapGroup

app.Run();

