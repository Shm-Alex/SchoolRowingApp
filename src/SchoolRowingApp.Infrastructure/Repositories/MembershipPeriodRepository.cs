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
    /*
Но оставляю так  чтобы не путать читателя
     Проблемы:
EF Core преобразует это в SQL с использованием функций для создания дат
Пример для PostgreSQL: WHERE MAKE_DATE("Year", "Month", 1) BETWEEN @p0 AND @p1
Не может эффективно использовать индекс по Year и Month
Требует вычисления даты для каждой строки таблицы
Приводит к full scan таблицы, даже если есть подходящий индекс
 оптимизация мб такой:
        int startMonthNumber = startDate.Year * 12 + startDate.Month;
        int endMonthNumber = endDate.Year * 12 + endDate.Month;

        return await _context.MembershipPeriods
        .Where(p => (p.Year * 12 + p.Month) >= startMonthNumber && 
                (p.Year * 12 + p.Month) <= endMonthNumber)
        .ToListAsync(cancellationToken);
     */
    public async Task<List<MembershipPeriod>> GetPeriodsInRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    => await _context.MembershipPeriods
            .Where(p => new DateTime(p.Year, p.Month, 1) >= startDate &&
                        new DateTime(p.Year, p.Month, 1) <= endDate)
            .ToListAsync(cancellationToken);
    
}