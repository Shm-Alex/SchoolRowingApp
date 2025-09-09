// Domain/Membership/AthleteMembership.cs
using SchoolRowingApp.Domain.SharedKernel;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Membership;

namespace SchoolRowingApp.Domain.Membership;

/// <summary>
/// Членство атлета в школе на определенный период (месяц).
/// Определяет коэффициент участия атлета и используется для расчета взноса.
/// </summary>
public class AthleteMembership : Entity
{
    /// <summary>
    /// ID атлета, который является членом школы.
    /// </summary>
    public Guid AthleteId { get; private set; }

    /// <summary>
    /// ID периода членства (месяц и год).
    /// </summary>
    public Guid MembershipPeriodId { get; private set; }

    /// <summary>
    /// Коэффициент участия (0, 0.5, 1).
    /// Определяет размер взноса атлета относительно базового взноса:
    /// - 0: Атлет не участвует в школе (в отпуске или приостановил членство)
    /// - 0.5: Малыши в ясельной группе (платят половину базового взноса)
    /// - 1: Штатный атлет (платит полный базовый взнос)
    /// </summary>
    public decimal ParticipationCoefficient { get; private set; }

    /// <summary>
    /// Навигационное свойство: атлет.
    /// Позволяет получить доступ к информации об атлете.
    /// </summary>
    public Athlete Athlete { get; private set; }

    /// <summary>
    /// Навигационное свойство: период членства.
    /// Позволяет получить доступ к информации о периоде и базовому взносу.
    /// </summary>
    public MembershipPeriod MembershipPeriod { get; private set; }

    /// <summary>
    /// Создает запись о членстве атлета на определенный период.
    /// </summary>
    /// <param name="athleteId">ID атлета</param>
    /// <param name="membershipPeriodId">ID периода членства</param>
    /// <param name="participationCoefficient">Коэффициент участия (0, 0.5, 1)</param>
    /// <exception cref="DomainException">Выбрасывается, если коэффициент участия недопустим</exception>
    public AthleteMembership(
        Guid athleteId,
        Guid membershipPeriodId,
        decimal participationCoefficient)
    {
        ValidateParticipationCoefficient(participationCoefficient);

        AthleteId = athleteId;
        MembershipPeriodId = membershipPeriodId;
        ParticipationCoefficient = participationCoefficient;
    }

    /// <summary>
    /// Обновляет коэффициент участия атлета.
    /// Используется при изменении статуса атлета (например, переход из яслей в основную группу).
    /// </summary>
    /// <param name="newCoefficient">Новый коэффициент участия</param>
    /// <exception cref="DomainException">Выбрасывается, если новый коэффициент недопустим</exception>
    public void UpdateParticipationCoefficient(decimal newCoefficient)
    {
        ValidateParticipationCoefficient(newCoefficient);
        ParticipationCoefficient = newCoefficient;
    }

    /// <summary>
    /// Валидация коэффициента участия.
    /// Проверяет, что коэффициент является одним из допустимых значений: 0, 0.5 или 1.
    /// </summary>
    /// <param name="coefficient">Коэффициент для проверки</param>
    /// <exception cref="DomainException">Выбрасывается при недопустимом коэффициенте</exception>
    private void ValidateParticipationCoefficient(decimal coefficient)
    {
        if (coefficient != 0 && coefficient != 0.5m && coefficient != 1)
            throw new DomainException("Коэффициент участия может быть только 0, 0.5 или 1");
    }

    /// <summary>
    /// Рассчитывает сумму взноса для этого периода.
    /// Формула: базовый взнос * коэффициент участия.
    /// Например: 2000 * 0.5 = 1000 рублей для малышей в ясельной группе.
    /// </summary>
    /// <returns>Сумма взноса в рублях</returns>
    public decimal CalculateFee()
    {
        return MembershipPeriod.BaseFee * ParticipationCoefficient;
    }
}