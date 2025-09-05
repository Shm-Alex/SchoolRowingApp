using SchoolRowingApp.Domain.SharedKernel;
using SchoolRowingApp.Domain.Payments;

namespace SchoolRowingApp.Domain.Athletes;

public class AthletePayer : Entity
{
    public Guid AthleteId { get; private set; }
    public Guid PayerId { get; private set; }
    public PayerType PayerType { get; private set; } // Мама, Папа и т.д.

    // Явные навигационные свойства
    public Athlete Athlete { get; private set; }
    public Payer Payer { get; private set; }
    private AthletePayer() { } // Для EF Core

    public AthletePayer(Guid athleteId, Guid payerId, PayerType payerType)
    {
        AthleteId = athleteId;
        PayerId = payerId;
        PayerType = payerType;
    }
}

public enum PayerType
{
    Self = 0,      // Сам атлет
    Mother = 1,    // Мама
    Father = 2,    // Папа
    Uncle = 3,     // Дядя
    Other = 4      // Другое
}