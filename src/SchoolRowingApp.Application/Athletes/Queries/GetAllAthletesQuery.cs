using MediatR;
using SchoolRowingApp.Application.Athletes.Dto;
using SchoolRowingApp.Domain.Athletes;
using System.Collections.Generic;

namespace SchoolRowingApp.Application.Athletes.Queries;

public record GetAllAthletesQuery : IRequest<List<AthleteDto>>;

public class GetAllAthletesQueryHandler :
    IRequestHandler<GetAllAthletesQuery, List<AthleteDto>>
{
    private readonly IAthleteRepository _athleteRepository;

    public GetAllAthletesQueryHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    public async Task<List<AthleteDto>> Handle(
        GetAllAthletesQuery request,
        CancellationToken ct)
    {
        var athletes = await _athleteRepository.GetAllAsync(ct);
        List<AthleteDto> athletesDto = athletes.Select(a => new AthleteDto() {Id=a.Id,FirstName=a.FirstName,LastName=a.LastName,SecondName=a.SecondName }).ToList();
        return await Task<List<AthleteDto>>.FromResult(athletesDto);
    }
}