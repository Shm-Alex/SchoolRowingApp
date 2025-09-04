using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Domain.Athletes;

public class Athlete : Entity
{
    public string FirstName { get; private set; }
    public string SecondName { get; private set; }
    public string LastName { get; private set; }

    private Athlete() { }

    public Athlete(string firstName, string secondName, string lastName)
    {
        ValidateName(firstName, "Имя");
        ValidateName(lastName, "Фамилия");

        FirstName = firstName;
        SecondName = secondName;
        LastName = lastName;
    }

    public void UpdateName(string firstName, string secondName, string lastName)
    {
        ValidateName(firstName, "Имя");
        ValidateName(lastName, "Фамилия");

        FirstName = firstName;
        SecondName = secondName;
        LastName = lastName;
    }

    private void ValidateName(string name, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException($"{fieldName} не может быть пустым");

        if (name.Length > 50)
            throw new DomainException($"{fieldName} не может быть длиннее 50 символов");
    }
}