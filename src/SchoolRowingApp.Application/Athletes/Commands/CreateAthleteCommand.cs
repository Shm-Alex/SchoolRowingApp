using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Athletes.Commands;

public record CreateAthleteCommand(
   // Guid Id,
    string FirstName,
    string SecondName,
    string LastName) : IRequest<Guid>;
public class CreateAthleteCommandHandler :
    IRequestHandler<CreateAthleteCommand, Guid>
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly AthleteDomainService _athleteDomainService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAthleteCommandHandler(
        IAthleteRepository athleteRepository,
        AthleteDomainService athleteDomainService,
        IUnitOfWork unitOfWork)
    {
        _athleteRepository = athleteRepository;
        _athleteDomainService = athleteDomainService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateAthleteCommand request,
        CancellationToken ct)
    {
        // Проверяем уникальность через доменный сервис
        if (!await _athleteDomainService.IsNameUniqueAsync(
            request.FirstName,
            request.SecondName,
            request.LastName,
            ct))
        {
            throw new Exception("Атлет с таким ФИО уже существует");
        }

        var athlete = new Athlete(
            request.FirstName,
            request.SecondName,
            request.LastName);

        await _athleteRepository.AddAsync(athlete, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return athlete.Id;
    }
}