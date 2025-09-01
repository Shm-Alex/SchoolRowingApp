namespace SchoolRowingApp.Domain.Entities
{
    public class Payer : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

    }
}