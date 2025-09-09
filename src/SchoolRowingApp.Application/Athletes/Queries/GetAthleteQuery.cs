using MediatR;
using SchoolRowingApp.Application.Athletes.Dto;
using SchoolRowingApp.Domain.Athletes;

namespace SchoolRowingApp.Application.Athletes.Queries;

public record GetAthleteQuery(Guid Id) : IRequest<AthleteDto>;

public class GetAthleteQueryHandler :
    IRequestHandler<GetAthleteQuery, AthleteDto>
{
    private readonly IAthleteRepository _athleteRepository;

    public GetAthleteQueryHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task<AthleteDto> Handle(
        GetAthleteQuery request,
        CancellationToken ct)
    {
        var athlete = await _athleteRepository.GetByIdAsync(request.Id, ct);
        if (athlete == null)
            throw new Exception("Атлет не найден");

        return new AthleteDto(athlete) ;
    }
}