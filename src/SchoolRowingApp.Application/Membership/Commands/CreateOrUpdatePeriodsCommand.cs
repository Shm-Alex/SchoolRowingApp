using Microsoft.Extensions.Logging;
using SchoolRowingApp.Application.Membership.Dto;
using SchoolRowingApp.Domain.Membership;
using SchoolRowingApp.Domain.SharedKernel;
/// <summary>
/// Команда для создания отсутствующих периодов членства в указанном диапазоне.
/// или обновления существующих
/// Создает новые периоды только для тех месяцев, которые отсутствуют в базе данных.
/// </summary>
public record CreateOrUpdatePeriodsCommand(
    decimal BaseFee,       // Базовый взнос для новых периодов
    int startMonth,        // Начальный месяц диапазона
    int startYear,         // Начальный год диапазона
    int endMonth,          // Конечный месяц диапазона
    int endYear            // Конечный год диапазона
) : IRequest<List<MembershipPeriodDto>>;
public abstract class CreateOrUpdateExistingPeriodsCommandHandler

{
    protected readonly IMembershipPeriodRepository _membershipPeriodRepository;
    protected readonly IUnitOfWork _unitOfWork;
    //protected readonly IMapper _mapper;
    protected readonly ILogger<CreateOrUpdateExistingPeriodsCommandHandler> _logger;
    public CreateOrUpdateExistingPeriodsCommandHandler(
        IMembershipPeriodRepository membershipPeriodRepository,
        IUnitOfWork unitOfWork,
    //    IMapper mapper,
        ILogger<CreateOrUpdateExistingPeriodsCommandHandler> logger)
    {
        _membershipPeriodRepository = membershipPeriodRepository;
        _unitOfWork = unitOfWork;
        //  _mapper = mapper;
        _logger = logger;
    }
    protected void ValidateRequestParameters(CreateOrUpdatePeriodsCommand request)
    {
        // Проверка корректности месяца (1-12)
        if (request.startMonth < 1 || request.startMonth > 12)
        {
            throw new DomainException("Начальный месяц должен быть в диапазоне от 1 до 12");
        }

        if (request.endMonth < 1 || request.endMonth > 12)
        {
            throw new DomainException("Конечный месяц должен быть в диапазоне от 1 до 12");
        }

        // Проверка, что начальная дата не позже конечной
        var startDate = new DateTime(request.startYear, request.startMonth, 1);
        var endDate = new DateTime(request.endYear, request.endMonth, 1);

        if (startDate > endDate)
        {
            throw new DomainException("Начальная дата не может быть позже конечной даты");
        }

        // Проверка, что базовый взнос положительный
        if (request.BaseFee <= 0)
        {
            throw new DomainException("Базовый взнос должен быть положительным числом");
        }


    }
}
