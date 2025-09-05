using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Domain.Athletes;

public interface IAthletePayerRepository
{
    Task<List<AthletePayer>> GetByAthleteIdAsync(Guid athleteId, CancellationToken ct);
    Task<List<AthletePayer>> GetByPayerIdAsync(Guid payerId, CancellationToken ct);
    Task AddAsync(AthletePayer athletePayer, CancellationToken ct);
    Task DeleteAsync(AthletePayer athletePayer, CancellationToken ct);
}