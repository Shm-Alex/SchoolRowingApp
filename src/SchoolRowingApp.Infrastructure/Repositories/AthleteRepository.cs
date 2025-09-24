using Microsoft.EntityFrameworkCore;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.SharedKernel;
using SchoolRowingApp.Infrastructure.Data;

namespace SchoolRowingApp.Infrastructure.Repositories;

public class AthleteRepository : IAthleteRepository
{
    private readonly ApplicationDbContext _context;

    public AthleteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Athlete> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Athletes
            .Include(a => a.AthletePayers)
            .ThenInclude(ap => ap.Payer)
            .Include(a => a.AthleteMemberships)
            .ThenInclude(am => am.MembershipPeriod)
            //.AsNoTracking() убираем  сущность нужна  для обновления  в update запросах
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }
    public async Task<int> GetAllCountAsync(CancellationToken ct)
    {
        return await _context.Athletes
            .AsNoTracking()
            .CountAsync();
    }
    public async Task<List<Athlete>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Athletes
            .Include(a=>a.AthletePayers)
            .ThenInclude(ap=>ap.Payer)
            .Include(a => a.AthleteMemberships)
            .ThenInclude(am => am.MembershipPeriod)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(Athlete athlete, CancellationToken ct)
    {
        await _context.Athletes.AddAsync(athlete, ct);
    }

    public async Task UpdateAsync(Athlete athlete, CancellationToken ct)
    {
         _context.Athletes.Update(athlete);
    }

    public async Task DeleteAsync(Athlete athlete, CancellationToken ct)
    {
        _context.Athletes.Remove(athlete);
    }

    public async Task<bool> IsNameUniqueAsync(string firstName, string secondName, string lastName, CancellationToken ct)
    {
     return ! await  _context.Athletes.AnyAsync
            (
                a => (a.FirstName == firstName) && (a.SecondName == secondName) && (a.LastName == lastName),ct
            );

    }

    public async Task<Athlete> GetByFullNameAsync(string firstName, string secondName, string lastName, CancellationToken ct)
    =>(await _context.Athletes
                        .AsNoTracking()
                        .FirstOrDefaultAsync(
                                            p =>
                                            p.FirstName == firstName &&
                                            p.SecondName == secondName &&
                                            p.LastName == lastName,ct))
        ;
            

}