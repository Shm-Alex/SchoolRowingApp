using MediatR;
using SchoolRowingApp.Domain.Athletes;
using System.Collections.Generic;

namespace SchoolRowingApp.Application.Athletes.Queries;

public record GetAllAthletesQuery : IRequest<List<Athlete>>;

public class GetAllAthletesQueryHandler :
    IRequestHandler<GetAllAthletesQuery, List<Athlete>>
{
    private readonly IAthleteRepository _athleteRepository;

    public GetAllAthletesQueryHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task<List<Athlete>> Handle(
        GetAllAthletesQuery request,
        CancellationToken ct)
    {
        return await _athleteRepository.GetAllAsync(ct);
    }
}