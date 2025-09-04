using MediatR;
using SchoolRowingApp.Domain.Athletes;

namespace SchoolRowingApp.Application.Athletes.Queries;

public record GetAthleteQuery(Guid Id) : IRequest<Athlete>;

public class GetAthleteQueryHandler :
    IRequestHandler<GetAthleteQuery, Athlete>
{
    private readonly IAthleteRepository _athleteRepository;

    public GetAthleteQueryHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task<Athlete> Handle(
        GetAthleteQuery request,
        CancellationToken ct)
    {
        var athlete = await _athleteRepository.GetByIdAsync(request.Id, ct);
        if (athlete == null)
            throw new Exception("Атлет не найден");

        return athlete;
    }
}