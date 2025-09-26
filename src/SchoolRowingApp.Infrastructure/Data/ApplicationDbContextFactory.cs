using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SchoolRowingApp.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /*
     // Infrastructure/DesignTimeDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SchoolRowingApp.Infrastructure;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        
        // Загружаем конфигурацию
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("src/SchoolRowingApp.WebApi/appsettings.json")
            .AddJsonFile("src/SchoolRowingApp.WebApi/appsettings.Local.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var defaultSchema = configuration["Database:DefaultSchema"] ?? "bob";

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseNpgsql(
            connectionString,
            npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", defaultSchema);
                npgsqlOptions.UseLowerCaseNamingConvention();
            }
        );

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
     */
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        
        // Определяем путь к корню проекта
        var basePath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
        if (string.IsNullOrEmpty(basePath))
        {
            basePath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName;
        }

        // Загружаем конфигурацию
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("src/SchoolRowingApp.WebApi/appsettings.json")
            .AddJsonFile("src/SchoolRowingApp.WebApi/appsettings.Local.json", optional: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var defaultSchema = configuration["Database:DefaultSchema"] ?? "bob";

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseNpgsql(
            connectionString,
            npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", defaultSchema);
               
            }
        );

        return new ApplicationDbContext(optionsBuilder.Options);
    }

}