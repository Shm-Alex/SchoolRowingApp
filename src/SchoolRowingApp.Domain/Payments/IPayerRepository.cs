using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Domain.Payments;

public interface IPayerRepository
{
    Task<Payer> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Payer> GetByFullNameAsync(string firstName, string secondName, string lastName, CancellationToken ct);
    Task<List<Payer>> GetAllAsync(CancellationToken ct);
    Task AddAsync(Payer payer, CancellationToken ct);
    Task UpdateAsync(Payer payer, CancellationToken ct);
    Task DeleteAsync(Payer payer, CancellationToken ct);
}