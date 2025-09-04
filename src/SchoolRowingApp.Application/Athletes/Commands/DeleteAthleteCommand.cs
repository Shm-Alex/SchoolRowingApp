using MediatR;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Athletes.Commands;

public record DeleteAthleteCommand(Guid Id) : IRequest;

public class DeleteAthleteCommandHandler :
    IRequestHandler<DeleteAthleteCommand>
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAthleteCommandHandler(
        IAthleteRepository athleteRepository,
        IUnitOfWork unitOfWork)
    {
        _athleteRepository = athleteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteAthleteCommand request,
        CancellationToken ct)
    {
        var athlete = await _athleteRepository.GetByIdAsync(request.Id, ct);
        if (athlete == null)
            throw new Exception("Атлет не найден");

        await _athleteRepository.DeleteAsync(athlete, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}