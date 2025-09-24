using MediatR;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Membership.Commands;

/// <summary>
/// Команда для удаления членства атлета на указанный период
/// </summary>
public record RemoveAthleteMembershipCommand(
Guid AthleteId, int MembershipPeriodMonth, int MembershipPeriodYear) : IRequest;

/// <summary>
/// Обработчик команды удаления членства атлета
/// </summary>
public class RemoveAthleteMembershipCommandHandler :
IRequestHandler<RemoveAthleteMembershipCommand>
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveAthleteMembershipCommandHandler(
    IAthleteRepository athleteRepository,
    IUnitOfWork unitOfWork)
    {
        _athleteRepository = athleteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
    RemoveAthleteMembershipCommand request,
    CancellationToken ct)
    {
        var athlete = await _athleteRepository.GetByIdAsync(request.AthleteId, ct);
        if (athlete == null)
            throw new Exception("Атлет не найден");

        athlete.RemoveMembership(request.MembershipPeriodMonth, request.MembershipPeriodYear);

        await _athleteRepository.UpdateAsync(athlete, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}