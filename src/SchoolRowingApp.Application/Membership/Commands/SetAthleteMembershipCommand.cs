// Application/Membership/Commands/SetAthleteMembershipCommand.cs
using MediatR;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Membership;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Membership.Commands;

/// <summary>
/// Команда для установки членства атлета на указанный период.
/// Используется администратором через UI для фиксации факта членства атлета.
/// </summary>
public record SetAthleteMembershipCommand(
    Guid AthleteId,
    int MembershipPeriodMonth,
    int MembershipPeriodYear,
    decimal ParticipationCoefficient) : IRequest;

/// <summary>
/// Обработчик команды установки членства атлета.
/// Обновляет доменную модель и сохраняет изменения в базу данных.
/// </summary>
public class SetAthleteMembershipCommandHandler :
    IRequestHandler<SetAthleteMembershipCommand>
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly IMembershipPeriodRepository _membershipPeriodRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetAthleteMembershipCommandHandler(
        IAthleteRepository athleteRepository,
        IMembershipPeriodRepository membershipPeriodRepository,
        IUnitOfWork unitOfWork)
    {
        _athleteRepository = athleteRepository;
        _membershipPeriodRepository = membershipPeriodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        SetAthleteMembershipCommand request,
        CancellationToken ct)
    {
        var athlete = await _athleteRepository.GetByIdAsync(request.AthleteId, ct);
        if (athlete == null)
            throw new Exception("Атлет не найден");

        var period = await _membershipPeriodRepository.GetByYearAndMonthAsync(request.MembershipPeriodMonth, request.MembershipPeriodYear, ct);
        if (period == null)
            throw new Exception("Период членства не найден");

        athlete.SetMembership(request.MembershipPeriodMonth, request.MembershipPeriodYear, request.ParticipationCoefficient);

        await _athleteRepository.UpdateAsync(athlete, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}