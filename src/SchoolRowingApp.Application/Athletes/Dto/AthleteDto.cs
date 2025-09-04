

namespace SchoolRowingApp.Application.Athletes.Dto;

public class AthleteDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string SecondName { get;  set; } = string.Empty;
    List<AthletePayerDto> Payers { get; set; }
}
public record AthletePayerDto(
                            Guid PayerId,
                            string FirstName,
                            string  LastName,
                            string SecondName ,
                            string PayerTypeDescription
);
