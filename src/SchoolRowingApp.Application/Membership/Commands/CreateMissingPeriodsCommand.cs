using Microsoft.Extensions.Logging;
using SchoolRowingApp.Application.Membership.Dto;
using SchoolRowingApp.Domain.Membership;

using SchoolRowingApp.Domain.SharedKernel;

/// <summary>
/// Команда для создания отсутствующих периодов членства в указанном диапазоне.
/// Создает новые периоды только для тех месяцев, которые отсутствуют в базе данных.
/// </summary>
public record CreateMissingPeriodsCommand(
    decimal BaseFee,       // Базовый взнос для новых периодов
    int startMonth,        // Начальный месяц диапазона
    int startYear,         // Начальный год диапазона
    int endMonth,          // Конечный месяц диапазона
    int endYear            // Конечный год диапазона
) : CreateOrUpdatePeriodsCommand(BaseFee,startMonth,startYear,endMonth,endYear);


/// <summary>
/// Обработчик команды создания отсутствующих периодов членства.
/// Создает новые периоды только для тех месяцев, которые отсутствуют в базе данных.
/// </summary>
public class CreateMissingPeriodsCommandHandler : CreateOrUpdateExistingPeriodsCommandHandler,
    IRequestHandler<CreateMissingPeriodsCommand, List<MembershipPeriodDto>>
{
    
    public CreateMissingPeriodsCommandHandler(
        IMembershipPeriodRepository membershipPeriodRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateMissingPeriodsCommandHandler> logger
        ):base(membershipPeriodRepository,unitOfWork, logger)
    {
        
    }
    /// <summary>
    /// Реализация обработчика
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<MembershipPeriodDto>> Handle(
        CreateMissingPeriodsCommand request,
        CancellationToken cancellationToken)
    {
        // Проверка параметров
        ValidateRequestParameters(request);

        // Создаем объекты DateTime для начала и конца диапазона
        DateTime startDate = new DateTime(request.startYear, request.startMonth, 1);
        DateTime endDate = new DateTime(request.endYear, request.endMonth, 1);

        // Получаем существующие периоды в указанном диапазоне
        var existingPeriods = await _membershipPeriodRepository.GetPeriodsInRangeAsync(
            startDate, endDate, cancellationToken);

        // Создаем список для новых периодов
        var newPeriods = new List<MembershipPeriod>();

        // Обрабатываем каждый месяц в диапазоне
        for (var date = startDate; date <= endDate; date = date.AddMonths(1))
        {
            // Проверяем, существует ли период
            if (!existingPeriods.Any(p => p.Year == date.Year && p.Month == date.Month))
            {
                var period = new MembershipPeriod(date.Month, date.Year, request.BaseFee);
                await _membershipPeriodRepository.AddAsync(period, cancellationToken);
                newPeriods.Add(period);
            }
        }

        if (!newPeriods.Any())
        {
            _logger.LogInformation(
                "Все периоды в диапазоне {StartMonth}/{StartYear}-{EndMonth}/{EndYear} уже существуют",
                request.startMonth, request.startYear, request.endMonth, request.endYear);
            return new List<MembershipPeriodDto>();
        }

        // Сохраняем изменения
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Возвращаем созданные периоды
        return newPeriods.Select(p => new MembershipPeriodDto(p)).ToList();
    }

}