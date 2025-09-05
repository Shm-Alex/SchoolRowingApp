using Microsoft.EntityFrameworkCore;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.SharedKernel;
using SchoolRowingApp.Infrastructure.Data;

namespace SchoolRowingApp.Infrastructure.Repositories;

public class AthletePayerRepository : IAthletePayerRepository
{
    private readonly ApplicationDbContext _context;

    public AthletePayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AthletePayer>> GetByAthleteIdAsync(Guid athleteId, CancellationToken ct)
    {
        return await _context.Set<AthletePayer>()
            .AsNoTracking()
            .Where(ap => ap.AthleteId == athleteId)
            .ToListAsync(ct);
    }

    public async Task<List<AthletePayer>> GetByPayerIdAsync(Guid payerId, CancellationToken ct)
    {
        return await _context.Set<AthletePayer>()
            .AsNoTracking()
            .Where(ap => ap.PayerId == payerId)
            .ToListAsync(ct);
    }

    public async Task AddAsync(AthletePayer athletePayer, CancellationToken ct)
    {
        await _context.AddAsync(athletePayer, ct);
    }

    public async Task DeleteAsync(AthletePayer athletePayer, CancellationToken ct)
    {
        _context.Remove(athletePayer);
    }
}