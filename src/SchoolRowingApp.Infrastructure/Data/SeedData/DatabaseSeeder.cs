// Infrastructure/Data/SeedData/DatabaseSeeder.cs
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Membership;
using SchoolRowingApp.Domain.Payments;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRowingApp.Infrastructure.Data.SeedData;

/// <summary>
/// Сервис для инициализации базы данных начальными данными.
/// Заполняет данные через доменные объекты, соблюдая бизнес-правила.
/// </summary>
public class DatabaseSeeder
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        IServiceProvider serviceProvider,
        ILogger<DatabaseSeeder> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    ///Потом доделаю 
}