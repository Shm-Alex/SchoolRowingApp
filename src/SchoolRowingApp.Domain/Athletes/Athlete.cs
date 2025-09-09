using SchoolRowingApp.Domain.Membership;
using SchoolRowingApp.Domain.Payments;
using SchoolRowingApp.Domain.SharedKernel;
using System.Collections.Generic;
using System.Linq;

namespace SchoolRowingApp.Domain.Athletes;

public class Athlete : Entity
{
    public string FirstName { get; private set; }
    public string SecondName { get; private set; }
    public string LastName { get; private set; }
    public DateTime Created { get; private set; }
    public DateTime? LastModified { get; private set; }

    private readonly List<AthletePayer> _athletePayers = new();
    public IReadOnlyList<AthletePayer> AthletePayers => _athletePayers.AsReadOnly();

    /// <summary>
    /// Навигационное свойство: членства атлета в разные периоды.
    /// Содержит историю участия атлета в школе по месяцам.
    /// </summary>
    private readonly List<AthleteMembership> _athleteMemberships = new();
    public IReadOnlyList<AthleteMembership> AthleteMemberships => _athleteMemberships.AsReadOnly();

    private Athlete() { }

    public Athlete(string firstName, string secondName, string lastName)
    {
        ValidateName(firstName, "Имя");
        ValidateName(lastName, "Фамилия");

        FirstName = firstName;
        SecondName = secondName;
        LastName = lastName;
        Created = DateTime.UtcNow;
    }

    public void UpdateName(string firstName, string secondName, string lastName)
    {
        ValidateName(firstName, "Имя");
        ValidateName(lastName, "Фамилия");

        if (FirstName != firstName ||
            SecondName != secondName ||
            LastName != lastName)
        {
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            UpdateLastModified();
        }
    }

    public void AddPayer(Guid payerId, PayerType payerType)
    {
        // Проверка на дубликат
        if (_athletePayers.Any(ap => ap.PayerId == payerId && ap.PayerType == payerType))
            throw new DomainException("Этот плательщик с такой ролью уже добавлен для атлета");

        _athletePayers.Add(new AthletePayer(Id, payerId, payerType));
        UpdateLastModified();
    }

    public void RemovePayer(Guid payerId, PayerType payerType)
    {
        var athletePayer = _athletePayers
            .FirstOrDefault(ap => ap.PayerId == payerId && ap.PayerType == payerType);

        if (athletePayer != null)
            _athletePayers.Remove(athletePayer);

        UpdateLastModified();
    }

    private void UpdateLastModified()
    {
        LastModified = DateTime.UtcNow;
    }

    private void ValidateName(string name, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException($"{fieldName} не может быть пустым");

        if (name.Length > 50)
            throw new DomainException($"{fieldName} не может быть длиннее 50 символов");
    }
    /// <summary>
    /// Добавляет или обновляет членство атлета на указанный период.
    /// Используется администратором для фиксации факта членства атлета в школе.
    /// </summary>
    /// <param name="membershipPeriodId">ID периода членства</param>
    /// <param name="participationCoefficient">Коэффициент участия (0, 0.5, 1)</param>
    /// <exception cref="DomainException">Выбрасывается, если коэффициент недопустим</exception>
    public void SetMembership(Guid membershipPeriodId, decimal participationCoefficient)
    {
        // Проверяем, существует ли уже запись для этого периода
        var existingMembership = _athleteMemberships
            .FirstOrDefault(m => m.MembershipPeriodId == membershipPeriodId);

        if (existingMembership != null)
        {
            // Обновляем существующую запись
            existingMembership.UpdateParticipationCoefficient(participationCoefficient);
        }
        else
        {
            // Создаем новую запись
            var membership = new AthleteMembership(Id, membershipPeriodId, participationCoefficient);
            _athleteMemberships.Add(membership);
            UpdateLastModified();
        }
    }

    /// <summary>
    /// Удаляет членство атлета на указанный период.
    /// Используется при удалении записи о членстве (например, при ошибке ввода).
    /// </summary>
    /// <param name="membershipPeriodId">ID периода членства</param>
    public void RemoveMembership(Guid membershipPeriodId)
    {
        var membership = _athleteMemberships
            .FirstOrDefault(m => m.MembershipPeriodId == membershipPeriodId);

        if (membership != null)
        {
            _athleteMemberships.Remove(membership);
            UpdateLastModified();
        }
    }

    /// <summary>
    /// Получает коэффициент участия для указанного периода.
    /// Возвращает null, если запись отсутствует.
    /// </summary>
    /// <param name="membershipPeriodId">ID периода членства</param>
    /// <returns>Коэффициент участия или null, если запись отсутствует</returns>
    public decimal? GetParticipationCoefficient(Guid membershipPeriodId)
    {
        var membership = _athleteMemberships
            .FirstOrDefault(m => m.MembershipPeriodId == membershipPeriodId);

        return membership?.ParticipationCoefficient;
    }

    /// <summary>
    /// Рассчитывает общий взнос за указанный период.
    /// Используется для отображения суммы, которую должен заплатить плательщик.
    /// </summary>
    /// <param name="membershipPeriod">Период членства</param>
    /// <returns>Сумма взноса в рублях</returns>
    public decimal CalculateFee(MembershipPeriod membershipPeriod)
    {
        var membership = _athleteMemberships
            .FirstOrDefault(m => m.MembershipPeriodId == membershipPeriod.Id);

        return membership != null ? membership.CalculateFee() : 0;
    }
}