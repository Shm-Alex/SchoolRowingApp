using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
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
    })
    ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

// Регистрируем MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(SchoolRowingApp.Application.Athletes.Commands.CreateAthleteCommand).Assembly));


var app = builder.Build();

// Применение миграций и инициализация данных
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        // Проверяем, существует ли таблица миграций
        bool migrationsTableExists = context.Database
            .SqlQueryRaw<bool>("SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_schema = 'bob' AND table_name = '__EFMigrationsHistory')")
            .AsEnumerable()
            .FirstOrDefault();

        if (!migrationsTableExists)
        {
            // Таблицы миграций нет, применяем все миграции
            logger.LogInformation("Таблица миграций не найдена. Применяем все миграции...");
            context.Database.Migrate();
        }
        else
        {
                logger.LogInformation("Применяем недостающие миграции...");

                // Получаем все миграции из сборки
                var appliedMigrations = context.Database.GetAppliedMigrations().ToList();
                var allMigrations = context.Database.GetMigrations().ToList();
                // Находим недостающие миграции
                var pendingMigrations = allMigrations.Except(appliedMigrations).ToList();
                var migrationsToRollback = appliedMigrations.Except(allMigrations).ToList();
                var migrationsToAppy = pendingMigrations.Any() ?
                    pendingMigrations :
                    migrationsToRollback.OrderByDescending(m => m).ToList();

                if (migrationsToAppy.Any())
                {
                    logger.LogInformation("Найдено {Count} непримененных миграций", migrationsToAppy.Count);

                    // Используем IMigrator для применения конкретных миграций
                    var migrator = context.GetService<IMigrator>();

                    // Применяем только недостающие миграции
                    foreach (var migration in migrationsToAppy)
                    {
                        logger.LogInformation("Применение миграции: {Migration}", migration);
                        migrator.Migrate(migration);
                    }
                }
                else
                {
                    logger.LogWarning("Миграции в истории присутствуют, но таблицы отсутствуют. Возможно, миграции были удалены или изменены.");

                    // Принудительно пересоздаем таблицу миграций
                    logger.LogInformation("Пересоздаем таблицу миграций...");

                    // Удаляем текущую таблицу миграций
                    context.Database.ExecuteSqlRaw("DROP TABLE IF EXISTS \"__EFMigrationsHistory\" CASCADE");

                    // Создаем заново
                    context.Database.Migrate();
                }
            //}
        }

        // Теперь инициализируем данные
        await seeder.InitializeAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ошибка при инициализации базы данных");
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