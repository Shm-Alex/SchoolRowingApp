namespace SchoolRowingApp.Application.Athletes.Dto;

public class AthleteDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PayerName { get; set; }
    public string SecondName { get;  set; } = string.Empty;
}