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
}