using SchoolRowingApp.Domain.Common;

namespace SchoolRowingApp.Domain.Entities;

public class Athlete : BaseEntity
{
	public string FirstName { get; set; } = string.Empty;
    public string SecondName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

	public Guid? PayerId { get; set; }
	public virtual Payer? Payer { get; set; }
}