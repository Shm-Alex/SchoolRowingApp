// Domain/Seed/AthleteSeedService.cs
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Payments;

namespace SchoolRowingApp.Domain.Seed;

/// <summary>
/// Доменный сервис для инициализации начальных данных.
/// Создает объекты через доменные сущности, соблюдая бизнес-правила.
/// </summary>
public class AthleteSeedService
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly IPayerRepository _payerRepository;

    public AthleteSeedService(
        IAthleteRepository athleteRepository,
        IPayerRepository payerRepository)
    {
        _athleteRepository = athleteRepository;
        _payerRepository = payerRepository;
    }

    /// <summary>
    /// Инициализирует систему начальными данными.
    /// Использует доменные объекты и методы, как при обычной работе через UI.
    /// </summary>
    public async Task InitializeAsync(CancellationToken ct = default)
    {
        // Создаем платильщиков через доменные объекты
        var payers = await CreatePayersAsync(ct);

        // Создаем атлетов через доменные объекты
        await CreateAthletesAsync(payers, ct);
    }

    private async Task<List<Payer>> CreatePayersAsync(CancellationToken ct)
    {
        var payers = new List<Payer>();

        // Группа 1: Головины
        var golovina = await CreatePayerAsync("Инна", "Владимировна", "Головина", ct);
        var shmikov = await CreatePayerAsync("Александр", "Дмитриевич", "Шмыков", ct);
        payers.AddRange(new[] { golovina, shmikov });

        // Группа 2: Кострюковы
        var kostriukova = await CreatePayerAsync("Ольга", "Александровна", "Кострюкова", ct);
        payers.Add(kostriukova);

        // Группа 3: Черногривовы
        var chernogrivov = await CreatePayerAsync("Игорь", "Евгеньевич", "Черногривов", ct);
        payers.Add(chernogrivov);

        // Остальные группы...

        return payers;
    }

    private async Task<Payer> CreatePayerAsync(
        string firstName,
        string secondName,
        string lastName,
        CancellationToken ct)
    {
        // Проверяем существование через репозиторий
        var existingPayer = await _payerRepository.GetByFullNameAsync(
            firstName, secondName, lastName, ct);

        if (existingPayer != null)
            return existingPayer;

        // Создаем через доменный объект (соблюдаем бизнес-правила)
        var payer = new Payer(firstName, secondName, lastName);
        await _payerRepository.AddAsync(payer, ct);

        return payer;
    }

    private async Task CreateAthletesAsync(
        List<Payer> payers,
        CancellationToken ct)
    {
        // Группа 1: Головины
        var golovin = await CreateAthleteWithPayersAsync(
            "Дмитрий", "Александрович", "Головин",
            new[] { (payers[0], PayerType.Mother), (payers[1], PayerType.Father) },
            ct);

        // Группа 2: Чебулаевы
        var chebulaeva = await CreateAthleteWithPayersAsync(
            "Вероника", "Дмитриевна", "Чебулаева",
            new[] { (payers[2], PayerType.Mother) },
            ct);

        // Группа 3: Черногривовы
        var chernogrivov = await CreateAthleteWithPayersAsync(
            "Леонид", "Игоревич", "Черногривов",
            new[] { (payers[3], PayerType.Father) },
            ct);

        // Остальные атлеты...
    }

    private async Task<Athlete> CreateAthleteWithPayersAsync(
        string firstName,
        string secondName,
        string lastName,
        IEnumerable<(Payer Payer, PayerType Type)> payers,
        CancellationToken ct)
    {
        // Создаем через доменный объект (соблюдаем бизнес-правила)
        var athlete = new Athlete(firstName, secondName, lastName);
        await _athleteRepository.AddAsync(athlete, ct);

        // Устанавливаем связи через доменные методы
        foreach (var (payer, type) in payers)
        {
            athlete.AddPayer(payer.Id, type);
        }

        await _athleteRepository.UpdateAsync(athlete, ct);
        return athlete;
    }
}