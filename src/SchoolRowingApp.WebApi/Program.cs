using Microsoft.EntityFrameworkCore;
using Npgsql;
using SchoolRowingApp.Infrastructure.Data;
using SchoolRowingApp.Infrastructure.Data.SeedData;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

// Устанавливаем UTF-8 кодировку для консоли
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

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

// ПРАВИЛЬНОЕ МЕСТО ДЛЯ ИНИЦИАЛИЗАЦИИ ДАННЫХ
// Применение миграций и инициализация данных
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>(); // <-- ДОБАВЛЕНО ПОЛУЧЕНИЕ SEEDER

    try
    {
        // Проверяем, есть ли незавершенные миграции
        if (context.Database.GetPendingMigrations().Any())
        {
            // Применяем миграции
            context.Database.Migrate();
            Console.WriteLine("Миграции успешно применены.");
        }
        else
        {
            Console.WriteLine("Миграции уже применены. Пропускаем...");
        }

        // ВСЕГДА ИНИЦИАЛИЗИРУЕМ ДАННЫЕ, ДАЖЕ ЕСЛИ МИГРАЦИИ УЖЕ ПРИМЕНЕНЫ
        await seeder.InitializeAsync(); 
    }
    catch (Exception ex) when (ex is NpgsqlException npgEx && npgEx.SqlState == "42P07")
    {
        // Таблица _EFMigrationsHistory уже существует — пропускаем миграции, но инициализируем данные
        Console.WriteLine("Таблица миграций уже существует. Пропускаем миграции...");
        await seeder.InitializeAsync(); // <-- ДОБАВЛЕН ВЫЗОВ ИНИЦИАЛИЗАЦИИ
    }
    catch (Exception ex)
    {
        // Логируем другие ошибки
        Console.WriteLine($"Ошибка инициализации: {ex.Message}");
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