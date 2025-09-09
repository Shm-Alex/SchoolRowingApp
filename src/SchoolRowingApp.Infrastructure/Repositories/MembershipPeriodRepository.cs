// Infrastructure/Repositories/MembershipPeriodRepository.cs
using Microsoft.EntityFrameworkCore;
using SchoolRowingApp.Domain.Membership;
using SchoolRowingApp.Domain.SharedKernel;
using SchoolRowingApp.Infrastructure.Data;

namespace SchoolRowingApp.Infrastructure.Repositories;

public class MembershipPeriodRepository : IMembershipPeriodRepository
{
    private readonly ApplicationDbContext _context;

    public MembershipPeriodRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MembershipPeriod> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.MembershipPeriods
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<MembershipPeriod> GetByYearAndMonthAsync(int year, int month, CancellationToken ct)
    {
        return await _context.MembershipPeriods
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Year == year && p.Month == month, ct);
    }

    public async Task<List<MembershipPeriod>> GetAllAsync(CancellationToken ct)
    {
        return await _context.MembershipPeriods
            .AsNoTracking()
            .OrderBy(p => p.Year).ThenBy(p => p.Month)
            .ToListAsync(ct);
    }

    public async Task AddAsync(MembershipPeriod period, CancellationToken ct)
    {
        await _context.MembershipPeriods.AddAsync(period, ct);
    }

    public async Task UpdateAsync(MembershipPeriod period, CancellationToken ct)
    {
        _context.MembershipPeriods.Update(period);
    }

    public async Task DeleteAsync(MembershipPeriod period, CancellationToken ct)
    {
        _context.MembershipPeriods.Remove(period);
    }
}