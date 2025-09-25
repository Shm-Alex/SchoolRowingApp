// Application/Athletes/Commands/CreateAthleteWithPayersCommand.cs
using MediatR;
using Microsoft.Extensions.Logging;
using SchoolRowingApp.Application.Athletes.Dto;
using SchoolRowingApp.Application.Membership.Dto;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Membership;
using SchoolRowingApp.Domain.Payments;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Athletes.Commands;

/// <summary>
/// Команда для создания атлета с плательщиками в одной транзакции.
/// Создает атлета, если он не существует, добавляет плательщиков (если они не существуют)
/// и устанавливает связи между ними.
/// </summary>
public record CreateAthleteWithPayersCommand(AthleteDto AthleteDto) : IRequest<Guid>;

/// <summary>
/// Обработчик команды создания атлета с плательщиками.
/// Выполняет все операции в одной транзакции, соблюдая бизнес-правила.
/// и периодами членства  MembershipPeriods 
/// </summary>
public class CreateAthleteWithPayersCommandHandler :
    IRequestHandler<CreateAthleteWithPayersCommand, Guid>
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly IPayerRepository _payerRepository;
    private readonly IAthleteMembershipRepository _athleteMembershipRepository;
    private readonly IMembershipPeriodRepository _membershipPeriodRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateAthleteWithPayersCommandHandler> _logger;

    public CreateAthleteWithPayersCommandHandler(
        IAthleteRepository athleteRepository,
        IPayerRepository payerRepository,
        IAthleteMembershipRepository athleteMembershipRepository,
        IMembershipPeriodRepository membershipPeriodRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateAthleteWithPayersCommandHandler> logger)
    {
        _athleteRepository = athleteRepository;
        _payerRepository = payerRepository;
        _athleteMembershipRepository= athleteMembershipRepository;
        _membershipPeriodRepository= membershipPeriodRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Guid> Handle(
        CreateAthleteWithPayersCommand request,
        CancellationToken ct)
    {
        _logger.LogInformation("Начало создания атлета с плательщиками: {FullName}",
            $"{request.AthleteDto.FirstName} {request.AthleteDto.LastName}");

        try
        {
            // Получаем или создаем атлета
            Athlete athlete;
            var existingAthlete = await _athleteRepository.GetByFullNameAsync(
                request.AthleteDto.FirstName,
                request.AthleteDto.SecondName,
                request.AthleteDto.LastName,
                ct);

            if (existingAthlete != null)
            {
                _logger.LogDebug("Атлет {FullName} уже существует",
                    $"{request.AthleteDto.FirstName} {request.AthleteDto.LastName}");
                athlete = existingAthlete;
            }
            else
            {
                _logger.LogInformation("Создание нового атлета: {FullName}",
                    $"{request.AthleteDto.FirstName} {request.AthleteDto.LastName}");
                athlete = new Athlete(
                    request.AthleteDto.FirstName,
                    request.AthleteDto.SecondName,
                    request.AthleteDto.LastName);
                await _athleteRepository.AddAsync(athlete, ct);
            }

            // Обрабатываем каждого плательщика
            foreach (var payerDto in request.AthleteDto.Payers)
            {
                await ProcessPayerForAthleteAsync(athlete, payerDto, ct);
            }
            // Обрабатываем и периоды членства  MembershipPeriods 
            foreach (AthleteMembershipDto membershipPeriodDto in request.AthleteDto.AthleteMemberships)
            {
                await ProcessMembershipPeriodForAthleteAsync(athlete, membershipPeriodDto, ct);
            }
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation("Атлет успешно создан с ID: {Id}", athlete.Id);
            return athlete.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании атлета с плательщиками");
            throw;
        }
    }

    private async Task ProcessMembershipPeriodForAthleteAsync(Athlete athlete, AthleteMembershipDto athleteMembershipDto, CancellationToken ct)
    {
        var _membershipPeriod = await _membershipPeriodRepository
            .GetByYearAndMonthAsync(athleteMembershipDto.MembershipPeriodYear, athleteMembershipDto.MembershipPeriodMonth, ct);
        if (_membershipPeriod == null)
        {
            throw new DomainException("Необходимо сначала добавить   в бд информацию о базовых взносах в SeedMembershipPeriodAsync");
        }

        athlete.SetMembership
            (athleteMembershipDto.MembershipPeriodMonth,
            athleteMembershipDto.MembershipPeriodYear,
            athleteMembershipDto.ParticipationCoefficient);
    }

    private async Task ProcessPayerForAthleteAsync(Athlete athlete, AthletePayerDto payerDto, CancellationToken ct)
    {
        // Получаем или создаем плательщика
        Payer payer;
        var existingPayer = await _payerRepository.GetByFullNameAsync(
            payerDto.FirstName,
            payerDto.SecondName,
            payerDto.LastName,
            ct);

        if (existingPayer != null)
        {
            _logger.LogDebug("Платильщик {FullName} уже существует",
                $"{payerDto.FirstName} {payerDto.LastName}");
            payer = existingPayer;
        }
        else
        {
            _logger.LogInformation("Создание нового плательщика: {FullName}",
                $"{payerDto.FirstName} {payerDto.LastName}");
            payer = new Payer(payerDto.FirstName, payerDto.SecondName, payerDto.LastName);
            await _payerRepository.AddAsync(payer, ct);
        }

        // Добавляем связь, если она еще не существует
        if (!athlete.AthletePayers.Any(ap => ap.PayerId == payer.Id))
        {
            if (Enum.TryParse<PayerType>(payerDto.PayerType, out var payerType))
            {
                _logger.LogInformation("Добавление связи атлет-{AthleteId} с платильщиком-{PayerId} как {PayerType}",
                    athlete.Id, payer.Id, payerType);
                athlete.AddPayer(payer.Id, payerType);
            }
            else
            {
                _logger.LogWarning("Невозможно распознать тип связи '{PayerType}' для платильщика {FullName}",
                    payerDto.PayerType, $"{payerDto.FirstName} {payerDto.LastName}");
            }
        }
        else
        {
            _logger.LogDebug("Связь атлет-{AthleteId} с платильщиком-{PayerId} уже существует",
                athlete.Id, payer.Id);
        }
    }
}