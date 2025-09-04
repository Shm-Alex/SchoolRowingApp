namespace SchoolRowingApp.Domain.Athletes;

public class AthleteDomainService
{
    private readonly IAthleteRepository _athleteRepository;

    public AthleteDomainService(IAthleteRepository athleteRepository)
    {
        _athleteRepository = athleteRepository;
    }

    // Пример доменной логики: проверка уникальности ФИО
    public async Task<bool> IsNameUniqueAsync(
        string firstName,
        string secondName,
        string lastName,
        CancellationToken ct = default)
    {
       return await _athleteRepository.IsNameUniqueAsync(firstName,
         secondName,
         lastName, ct);
    }
}