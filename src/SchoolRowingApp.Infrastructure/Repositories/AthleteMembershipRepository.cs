// Infrastructure/Repositories/AthleteMembershipRepository.cs
using Microsoft.EntityFrameworkCore;
using SchoolRowingApp.Domain.Membership;
using SchoolRowingApp.Domain.SharedKernel;
using SchoolRowingApp.Infrastructure.Data;

namespace SchoolRowingApp.Infrastructure.Repositories;

public class AthleteMembershipRepository : IAthleteMembershipRepository
{
    private readonly ApplicationDbContext _context;

    public AthleteMembershipRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AthleteMembership> GetByIdAsync(Guid id, CancellationToken ct)
    {
        // Для составного ключа используем другой подход
        throw new NotImplementedException("Используется составной первичный ключ, метод не применим");
    }

    public async Task<List<AthleteMembership>> GetByAthleteIdAsync(Guid athleteId, CancellationToken ct)
    {
        return await _context.AthleteMemberships
            .AsNoTracking()
            .Include(am => am.MembershipPeriod)
            .Where(am => am.AthleteId == athleteId)
            .ToListAsync(ct);
    }

    public async Task<List<AthleteMembership>> GetByPeriodAsync( int membershipPeriodMonth, int membershipPeriodYear, CancellationToken ct)
    {
        return await _context.AthleteMemberships
            .AsNoTracking()
            .Include(am => am.Athlete)
            .Where(am => (am.MembershipPeriodMonth == membershipPeriodMonth)&& (am.MembershipPeriodYear == membershipPeriodYear))
            .ToListAsync(ct);
    }

    public async Task AddAsync(AthleteMembership membership, CancellationToken ct)
    {
        await _context.AthleteMemberships.AddAsync(membership, ct);
    }

    public async Task UpdateAsync(AthleteMembership membership, CancellationToken ct)
    {
        _context.AthleteMemberships.Update(membership);
    }

    public async Task DeleteAsync(AthleteMembership membership, CancellationToken ct)
    {
        _context.AthleteMemberships.Remove(membership);
    }
}