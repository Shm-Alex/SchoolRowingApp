using MediatR;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Athletes.Commands;

public record RemovePayerFromAthleteCommand(
    Guid AthleteId,
    Guid PayerId,
    PayerType PayerType) : IRequest;

public class RemovePayerFromAthleteCommandHandler :
    IRequestHandler<RemovePayerFromAthleteCommand>
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemovePayerFromAthleteCommandHandler(
        IAthleteRepository athleteRepository,
        IUnitOfWork unitOfWork)
    {
        _athleteRepository = athleteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        RemovePayerFromAthleteCommand request,
        CancellationToken ct)
    {
        var athlete = await _athleteRepository.GetByIdAsync(request.AthleteId, ct);
        if (athlete == null)
            throw new Exception("Атлет не найден");

        athlete.RemovePayer(request.PayerId, request.PayerType);

        await _athleteRepository.UpdateAsync(athlete, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}