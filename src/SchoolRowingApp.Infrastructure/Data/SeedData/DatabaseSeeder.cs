// Infrastructure/Data/SeedData/DatabaseSeeder.cs
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SchoolRowingApp.Application.Athletes.Commands;
using SchoolRowingApp.Application.Athletes.Dto;
using SchoolRowingApp.Application.Athletes.Queries;
using SchoolRowingApp.Application.Payments.Commands;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Membership;
using SchoolRowingApp.Domain.Payments;
using SchoolRowingApp.Infrastructure.Repositories;
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
    /// <summary>
    /// Инициализирует базу данных начальными данными через CQRS команды.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Задача</returns>
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Начало инициализации базы данных начальными данными через CQRS команды");

        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var seedMembershipPeriodAsync = SeedMembershipPeriodAsync(mediator, cancellationToken);
        var seededCount=await SeedAthletesAsync(mediator, cancellationToken);
        _logger.LogInformation($"Добавлено {seededCount} атлетов");
    }

    /// <summary>
    /// Инициализирует периоды членства с разными базовыми взносами
    /// </summary>
    private async Task<List<Application.Membership.Dto.MembershipPeriodDto>> SeedMembershipPeriodAsync(IMediator mediator, CancellationToken ct)
    {
        try
        {
            
            _logger.LogInformation("Инициализация  недостающих периодов  Периоды с базовым взносом 2000 рублей: февраль 2024 - август 2025 ");
            List<Application.Membership.Dto.MembershipPeriodDto> membership2025 = await mediator.Send(new CreateMissingPeriodsCommand(2000, 2, 2024, 8, 2025), ct);
            _logger.LogInformation("Инициализация  недостающих периодов Периоды с базовым взносом 3000 рублей: сентябрь 2025 - сентябрь 2026");
            List<Application.Membership.Dto.MembershipPeriodDto> membership2026 = await mediator.Send(new CreateMissingPeriodsCommand(2000, 9, 2025, 9, 2026), ct);
            _logger.LogInformation("Инициализация  недостающих периодов  базовых взосов завершена. добавлено записей {Count}", membership2025.Count()+membership2026.Count());
            return membership2025.Union(membership2026).ToList();

        }
        catch(Exception ex)
        {
             _logger.LogError("Инициализация  недостающих периодов  базовых взосов завершена, с ошибкой ", ex);
            throw;
        }
    }

    private async Task<int> SeedAthletesAsync(IMediator mediator, CancellationToken ct)
    {
        

        var existingAthleteCount = await mediator.Send(new GetAthletesCountQuery(), ct);

        if (existingAthleteCount >= AthleteWithPayersInitialData.Length)
        {
            _logger.LogInformation("Данные уже инициализированы. Существует {Count} атлетов, требуется {Required}",
                existingAthleteCount, AthleteWithPayersInitialData.Length);
            return 0;
        }

        _logger.LogInformation("Начало инициализации данных. Существует {Count} атлетов из {Required}",
            existingAthleteCount, AthleteWithPayersInitialData.Length);

        int seededCount = 0;

        foreach (var athleteDto in AthleteWithPayersInitialData)
        {
            try
            {
                await mediator.Send(new CreateAthleteWithPayersCommand(athleteDto), ct);
                seededCount++;
                _logger.LogInformation("Успешно инициализирован атлет: {FullName}",
                    $"{athleteDto.FirstName} {athleteDto.LastName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при инициализации атлета {FullName}",
                    $"{athleteDto.FirstName} {athleteDto.LastName}");
            }
        }

        _logger.LogInformation("Инициализация завершена. Добавлено {Count} атлетов", seededCount);
        return seededCount;
    }
 
    #region initial data
    AthleteDto[] AthleteWithPayersInitialData => new AthleteDto[]
 {
    new AthleteDto()
    {
        FirstName = "Дмитрий",
        SecondName = "Александрович",
        LastName = "Головин",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Инна",
                SecondName = "Владимировна",
                LastName = "Головина",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            },
            new AthletePayerDto
            {
                FirstName = "Александр",
                SecondName = "Дмитриевич",
                LastName = "Шмыков",
                PayerType = PayerType.Father.ToString(),
                PayerTypeDescription = "Папа"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Вероника",
        SecondName = "Дмитриевна",
        LastName = "Чебулаева",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Ольга",
                SecondName = "Александровна",
                LastName = "Кострюкова",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Леонид",
        SecondName = "Игоревич",
        LastName = "Черногривов",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Игорь",
                SecondName = "Евгеньевич",
                LastName = "Черногривов",
                PayerType = PayerType.Father.ToString(),
                PayerTypeDescription = "Папа"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Александр",
        SecondName = "Андреевич",
        LastName = "Шишлин",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Жанна",
                SecondName = "Анатольевна",
                LastName = "Шишлина",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            },
            new AthletePayerDto
            {
                FirstName = "Андрей",
                SecondName = "Александрович",
                LastName = "Шишлин",
                PayerType = PayerType.Father.ToString(),
                PayerTypeDescription = "Папа"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Никита",
        SecondName = "Сергеевич",
        LastName = "Резников",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Виктория",
                SecondName = "Михайловна",
                LastName = "Резникова",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            },
            new AthletePayerDto
            {
                FirstName = "Сергей",
                SecondName = "Владимирович",
                LastName = "Резников",
                PayerType = PayerType.Father.ToString(),
                PayerTypeDescription = "Папа"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Ксения",
        SecondName = "Антоновна",
        LastName = "Жгун",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Екатерина",
                SecondName = "Борисовна",
                LastName = "Качер",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Елисей",
        SecondName = "Петрович",
        LastName = "Ивлиев",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Оксана",
                SecondName = "Андреевна",
                LastName = "Ивлиева",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            },
            new AthletePayerDto
            {
                FirstName = "Петр",
                SecondName = "Павлович",
                LastName = "Ивлиев",
                PayerType = PayerType.Father.ToString(),
                PayerTypeDescription = "Папа"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Егор",
        SecondName = "Петрович",
        LastName = "Ивлиев",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Оксана",
                SecondName = "Андреевна",
                LastName = "Ивлиева",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            },
            new AthletePayerDto
            {
                FirstName = "Петр",
                SecondName = "Павлович",
                LastName = "Ивлиев",
                PayerType = PayerType.Father.ToString(),
                PayerTypeDescription = "Папа"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Макар",
        SecondName = "Петрович",
        LastName = "Ивлиев",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Оксана",
                SecondName = "Андреевна",
                LastName = "Ивлиева",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            },
            new AthletePayerDto
            {
                FirstName = "Петр",
                SecondName = "Павлович",
                LastName = "Ивлиев",
                PayerType = PayerType.Father.ToString(),
                PayerTypeDescription = "Папа"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Тимур",
        SecondName = "Наилевич",
        LastName = "Сулейманов",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Ольга",
                SecondName = "Александровна",
                LastName = "Сулейманова",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            },
            new AthletePayerDto
            {
                FirstName = "Наиль",
                SecondName = string.Empty,
                LastName = "Сулейманов",
                PayerType = PayerType.Father.ToString(),
                PayerTypeDescription = "Папа"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Дмитрий",
        SecondName = "Александрович",
        LastName = "Никишин",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Екатерина",
                SecondName = "Викторовна",
                LastName = "Никишина",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Маргарита",
        SecondName = "Сергеевна",
        LastName = "Васильева",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Марина",
                SecondName = "Сергеевна",
                LastName = "Шапкина",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Серафим",
        SecondName = "Владимирович",
        LastName = "Крылов",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Ксения",
                SecondName = "Вадимовна",
                LastName = "Крылова",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Демид",
        SecondName = "Вадимович",
        LastName = "Суханов",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Демид",
                SecondName = "Вадимович",
                LastName = "Суханов",
                PayerType = PayerType.Self.ToString(),
                PayerTypeDescription = "Сам атлет"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Егор",
        SecondName = "Юрьевич",
        LastName = "Чеканов",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Арина",
                SecondName = "Николаевна",
                LastName = "Новикова",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            },
            new AthletePayerDto
            {
                FirstName = "Юрий",
                SecondName = "Александрович",
                LastName = "Чеканов",
                PayerType = PayerType.Father.ToString(),
                PayerTypeDescription = "Папа"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Максим",
        SecondName = "Русланович",
        LastName = "Нуреев",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Руслан",
                SecondName = "Саярович",
                LastName = "Нуреев",
                PayerType = PayerType.Father.ToString(),
                PayerTypeDescription = "Папа"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Артём",
        SecondName = "Русланович",
        LastName = "Нуреев",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Руслан",
                SecondName = "Саярович",
                LastName = "Нуреев",
                PayerType = PayerType.Father.ToString(),
                PayerTypeDescription = "Папа"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Тимур",
        SecondName = "Мохамедович",
        LastName = "Хашиша",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Настасья",
                SecondName = "Юрьевна",
                LastName = "Макарова",
                PayerType = PayerType.Mother.ToString(),
                PayerTypeDescription = "Мама"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Степан",
        SecondName = string.Empty,
        LastName = "Ромашкин",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Дмитрий",
                SecondName = "Викторович",
                LastName = "Ромашкин",
                PayerType = PayerType.Self.ToString(),
                PayerTypeDescription = "Сам атлет"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Платон",
        SecondName = "Иванов",
        LastName = "Кондратенко",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Полина",
                SecondName = "Сергеевна",
                LastName = "Кондратенко",
                PayerType = PayerType.Self.ToString(),
                PayerTypeDescription = "Сам атлет"
            }
        }
    },
    new AthleteDto()
    {
        FirstName = "Максим",
        SecondName = string.Empty,
        LastName = "Мирошниченнко",
        Payers = new List<AthletePayerDto>
        {
            new AthletePayerDto
            {
                FirstName = "Екатерина",
                SecondName = "Александровна",
                LastName = "Мирошниченко",
                PayerType = PayerType.Self.ToString(),
                PayerTypeDescription = "Сам атлет"
            }
        }
    }
 };
    #endregion
    ///Потом доделаю 
    
}