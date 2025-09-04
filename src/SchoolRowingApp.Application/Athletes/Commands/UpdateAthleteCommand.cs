using MediatR;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Athletes.Commands;
public record UpdateAthleteCommand(
    Guid Id,
    string FirstName,
    string SecondName,
    string LastName) : IRequest;

public class UpdateAthleteCommandHandler :
    IRequestHandler<UpdateAthleteCommand>
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly AthleteDomainService _athleteDomainService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAthleteCommandHandler(
        IAthleteRepository athleteRepository,
        AthleteDomainService athleteDomainService,
        IUnitOfWork unitOfWork)
    {
        _athleteRepository = athleteRepository;
        _athleteDomainService = athleteDomainService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateAthleteCommand request,
        CancellationToken ct)
    {
        var athlete = await _athleteRepository.GetByIdAsync(request.Id, ct);
        if (athlete == null)
            throw new Exception("Атлет не найден");

        // Проверяем уникальность через доменный сервис
        if (!await _athleteDomainService.IsNameUniqueAsync(
            request.FirstName,
            request.SecondName,
            request.LastName,
            ct))
        {
            throw new Exception("Атлет с таким ФИО уже существует");
        }

        athlete.UpdateName(
            request.FirstName,
            request.SecondName,
            request.LastName);

        await _athleteRepository.UpdateAsync(athlete, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}