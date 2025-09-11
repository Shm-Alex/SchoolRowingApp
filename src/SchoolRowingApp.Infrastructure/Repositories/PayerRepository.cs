using Microsoft.EntityFrameworkCore;
using SchoolRowingApp.Domain.Payments;
using SchoolRowingApp.Domain.SharedKernel;
using SchoolRowingApp.Infrastructure.Data;

namespace SchoolRowingApp.Infrastructure.Repositories;

public class PayerRepository : IPayerRepository
{
    private readonly ApplicationDbContext _context;

    public PayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Payer> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Payers
             .Include(p => p.AthletePayers) // Загружаем список связей
            .ThenInclude(ap => ap.Athlete) // Для каждой связи загружаем атлета
            .AsNoTracking() 
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<Payer> GetByFullNameAsync(
        string firstName,
        string secondName,
        string lastName,
        CancellationToken ct)
    {
        return await _context.Payers
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                p.FirstName == firstName &&
                p.SecondName == secondName &&
                p.LastName == lastName,
                ct);
    }

    public async Task<List<Payer>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Payers
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(Payer payer, CancellationToken ct)
    {
        await _context.Payers.AddAsync(payer, ct);
    }

    public async Task UpdateAsync(Payer payer, CancellationToken ct)
    {
        _context.Payers.Update(payer);
    }

    public async Task DeleteAsync(Payer payer, CancellationToken ct)
    {
        _context.Payers.Remove(payer);
    }
}