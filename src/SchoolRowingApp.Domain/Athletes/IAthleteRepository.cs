using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Domain.Athletes;

public interface IAthleteRepository
{
    Task<Athlete> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Athlete>> GetAllAsync(CancellationToken ct);
    Task AddAsync(Athlete athlete, CancellationToken ct);
    Task UpdateAsync(Athlete athlete, CancellationToken ct);
    Task DeleteAsync(Athlete athlete, CancellationToken ct);
    Task<bool> IsNameUniqueAsync(string firstName, string secondName, string lastName, CancellationToken ct);
}