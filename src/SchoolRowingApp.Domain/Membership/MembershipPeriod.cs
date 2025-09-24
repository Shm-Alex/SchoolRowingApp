// Domain/Membership/MembershipPeriod.cs
using SchoolRowingApp.Domain.SharedKernel;
using System.ComponentModel.DataAnnotations;

namespace SchoolRowingApp.Domain.Membership;

/// <summary>
/// Календарь  с записями о базовом взносе. месяц - год- величина базового взноса в этом месяце 
/// Служит основой для расчета ежемесячных платежей атлетов.
/// </summary>
public class MembershipPeriod : CompositeKeyEntity
{
    [Key]
    /// <summary>
    /// Месяц (1-12), для которого устанавливается базовый взнос.
    /// </summary>
    public int Month { get; private set; }
    [Key]
    /// <summary>
    /// Год, для которого устанавливается базовый взнос.
    /// </summary>
    public int Year { get; private set; }

    /// <summary>
    /// Базовый взнос за месяц в рублях.
    /// Например: 2000 рублей в 2023 году, 3000 рублей в 2024 году.
    /// Этот взнос используется для расчета платежей атлетов с учетом их коэффициента участия.
    /// </summary>
    public decimal BaseFee { get; private set; }

    /// <summary>
    /// Навигационное свойство: список членств атлетов в этот период.
    /// Содержит записи о том, какие атлеты являются членами школы в данный период
    /// и с каким коэффициентом участия.
    /// </summary>

    private readonly List<AthleteMembership> _athleteMemberships = new();

    // Сделайте свойство виртуальным для поддержки lazy-loading
    public virtual IReadOnlyList<AthleteMembership> AthleteMemberships =>
        _athleteMemberships.AsReadOnly();

    /// <summary>
    /// Создает новый период членства.
    /// </summary>
    /// <param name="month">Месяц (1-12)</param>
    /// <param name="year">Год</param>
    /// <param name="baseFee">Базовый взнос за месяц в рублях</param>
    /// <exception cref="DomainException">Выбрасывается, если месяц выходит за пределы 1-12,
    /// год находится вне разумного диапазона или базовый взнос отрицательный</exception>
    public MembershipPeriod(int month, int year, decimal baseFee)
    {
        ValidatePeriod(month, year);
        ValidateBaseFee(baseFee);

        Month = month;
        Year = year;
        BaseFee = baseFee;
    }

    /// <summary>
    /// Обновляет базовый взнос для периода членства.
    /// Используется при изменении стоимости членства в будущем.
    /// Например, при переходе с 2000 рублей на 3000 рублей с нового года.
    /// </summary>
    /// <param name="newBaseFee">Новый базовый взнос</param>
    /// <exception cref="DomainException">Выбрасывается, если новый базовый взнос отрицательный</exception>
    public void UpdateBaseFee(decimal newBaseFee)
    {
        ValidateBaseFee(newBaseFee);
        BaseFee = newBaseFee;
    }

    /// <summary>
    /// Валидация периода (месяц и год).
    /// Проверяет, что месяц находится в диапазоне 1-12, а год в разумных пределах.
    /// </summary>
    /// <param name="month">Месяц для проверки</param>
    /// <param name="year">Год для проверки</param>
    /// <exception cref="DomainException">Выбрасывается при невалидных значениях</exception>
    private void ValidatePeriod(int month, int year)
    {
        if (month < 1 || month > 12)
            throw new DomainException("Месяц должен быть в диапазоне от 1 до 12");

        if (year < 2020 || year > 2100)
            throw new DomainException("Год должен быть в разумном диапазоне (2020-2100)");
    }

    /// <summary>
    /// Валидация базового взноса.
    /// Проверяет, что базовый взнос не отрицательный.
    /// </summary>
    /// <param name="baseFee">Базовый взнос для проверки</param>
    /// <exception cref="DomainException">Выбрасывается, если базовый взнос отрицательный</exception>
    private void ValidateBaseFee(decimal baseFee)
    {
        if (baseFee < 0)
            throw new DomainException("Базовый взнос не может быть отрицательным");
    }

    /// <summary>
    /// Получает дату начала периода (первый день месяца).
    /// Например, для января 2024: 01.01.2024.
    /// </summary>
    public DateTime StartDate => new(Year, Month, 1);

    /// <summary>
    /// Получает дату окончания периода (последний день месяца).
    /// Например, для января 2024: 31.01.2024.
    /// </summary>
    public DateTime EndDate => StartDate.AddMonths(1).AddDays(-1);

    /// <summary>
    /// Проверяет, относится ли указанная дата к этому периоду членства.
    /// </summary>
    /// <param name="date">Дата для проверки</param>
    /// <returns>true, если дата находится в пределах этого периода</returns>
    public bool ContainsDate(DateTime date) =>
        date >= StartDate && date <= EndDate;
}