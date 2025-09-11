using SchoolRowingApp.Application.Athletes.Dto;
using SchoolRowingApp.Domain.Athletes;

namespace SchoolRowingApp.Application.Athletes.Queries;

public record GetAthletesCountQuery: IRequest<int>;
public class GetAllAthletesCountQueryHandler :
    IRequestHandler<GetAthletesCountQuery, int>
{
    private readonly IAthleteRepository _athleteRepository;

    public GetAllAthletesCountQueryHandler(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }


    public async Task<int> Handle(GetAthletesCountQuery request, CancellationToken cancellationToken)
        =>await  _athleteRepository.GetAllCountAsync(cancellationToken);
}