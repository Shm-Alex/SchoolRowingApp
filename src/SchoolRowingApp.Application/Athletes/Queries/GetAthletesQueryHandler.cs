using SchoolRowingApp.Application.Athletes.Dto;
using SchoolRowingApp.Application.Common.Interfaces;

namespace SchoolRowingApp.Application.Athletes.Queries.GetAthletes;

public class GetAthletesQueryHandler : IRequestHandler<GetAthletesQuery, List<AthleteDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAthletesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AthleteDto>> Handle(GetAthletesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Athletes
            .Include(a => a.Payer)
            .Select(a => new AthleteDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                SecondName = a.SecondName,
                LastName = a.LastName,
                PayerName = a.Payer != null ? $"{a.Payer.FirstName} {a.Payer.LastName[0]}" : null
            })
            .ToListAsync(cancellationToken);
    }
}