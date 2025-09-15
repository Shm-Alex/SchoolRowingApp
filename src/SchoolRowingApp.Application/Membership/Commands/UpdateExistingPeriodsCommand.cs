using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRowingApp.Application.Membership.Dto;
using SchoolRowingApp.Domain.Membership;

using SchoolRowingApp.Domain.SharedKernel;

/// <summary>
/// Команда для обновления базовых взносов существующих периодов членства.
/// Обновляет только те периоды, которые уже существуют в базе данных.
/// </summary>
public record UpdateExistingPeriodsCommand(
    decimal BaseFee,    // Новое значение базового взноса
    int startMonth,        // Начальный месяц диапазона
    int startYear,         // Начальный год диапазона
    int endMonth,          // Конечный месяц диапазона
    int endYear            // Конечный год диапазона
) :CreateOrUpdatePeriodsCommand(BaseFee, startMonth, startYear, endMonth, endYear);

/// <summary>
/// Обработчик команды обновления существующих периодов членства.
/// Обновляет базовый взнос только для существующих периодов в указанном диапазоне.
/// </summary>
public class UpdateExistingPeriodsCommandHandler : CreateOrUpdateExistingPeriodsCommandHandler,
    IRequestHandler<UpdateExistingPeriodsCommand, List<MembershipPeriodDto>>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="membershipPeriodRepository"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="logger"></param>
    public UpdateExistingPeriodsCommandHandler(IMembershipPeriodRepository membershipPeriodRepository,
        IUnitOfWork unitOfWork,
        //IMapper mapper,
        ILogger<UpdateExistingPeriodsCommandHandler> logger
        ) :base(membershipPeriodRepository,
            unitOfWork, 
            //mapper            , 
            logger)
    {
    }
    // Реализация обработчика
    public async Task<List<MembershipPeriodDto>> Handle(
        UpdateExistingPeriodsCommand request,
        CancellationToken cancellationToken)
    {
        // Проверка параметров
        ValidateRequestParameters(request);

        // Создаем объекты DateTime для начала и конца диапазона
        DateTime startDate = new DateTime(request.startYear, request.startMonth, 1);
        DateTime endDate = new DateTime(request.endYear, request.endMonth, 1);

        // Получаем существующие периоды в указанном диапазоне
        var periodsToUpdate = await _membershipPeriodRepository.GetPeriodsInRangeAsync(
            startDate, endDate, cancellationToken);

        if (!periodsToUpdate.Any())
        {
            _logger.LogWarning(
                "Не найдено периодов членства в диапазоне {StartMonth}/{StartYear}-{EndMonth}/{EndYear}",
                request.startMonth, request.startYear, request.endMonth, request.endYear);
            return new List<MembershipPeriodDto>();
        }

        // Обновляем базовый взнос для каждого периода
        foreach (var period in periodsToUpdate)
        {
            if (period.BaseFee != request.BaseFee)
            {
                period.UpdateBaseFee(request.BaseFee);
            }
        }

        // Сохраняем изменения
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Возвращаем обновленные периоды
        return periodsToUpdate.Select(pu=>new MembershipPeriodDto(pu)).ToList();
    }

   
}