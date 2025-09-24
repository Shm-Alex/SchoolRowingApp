

using SchoolRowingApp.Application.Membership.Dto;
using SchoolRowingApp.Domain.Athletes;

namespace SchoolRowingApp.Application.Athletes.Dto;


/// <summary>
/// DTO для представления атлета в системе.
/// Используется для передачи данных об атлете через API.
/// Содержит основную информацию об атлете и список связанных плательщиков.
/// а также создаёт  информацию о членстве в школе MembershipPeriods 
/// </summary>
public class AthleteDto
{
    /// <summary>
    /// Инициализирует новый экземпляр класса AthleteDto на основе доменной сущности Athlete.
    /// Преобразует доменную модель в DTO, включая информацию о плательщиках.
    /// </summary>
    /// <param name="a">Доменная сущность атлета, из которой будут скопированы данные</param>
    public AthleteDto(Athlete a)
    {
        Id = a.Id;
        FirstName = a.FirstName;
        LastName = a.LastName;
        SecondName = a.SecondName;
        Payers = a.AthletePayers.Select(ap => ap.ToAthletePayerDto()).ToList();
        AthleteMemberships=a.AthleteMemberships.Select(mp=> new AthleteMembershipDto(mp)).ToList();
    }
    public AthleteDto() { }


    /// <summary>
    /// Уникальный идентификатор атлета.
    /// Используется для однозначной идентификации атлета в системе.
    /// Генерируется автоматически при создании атлета.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя атлета.
    /// Должно быть непустой строкой и содержать корректное имя.
    /// Используется для идентификации атлета в системе.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Фамилия атлета.
    /// Должна быть непустой строкой и содержать корректную фамилию.
    /// Используется для идентификации атлета в системе.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Отчество атлета.
    /// Может быть пустой строкой (например, для иностранных граждан или лиц без отчества).
    /// Используется для более точной идентификации атлета.
    /// </summary>
    public string SecondName { get; set; } = string.Empty;

    /// <summary>
    /// Список плательщиков, связанных с атлетом.
    /// Каждый плательщик имеет определенную роль (мама, папа, сам атлет и т.д.).
    /// Используется для отображения информации о финансировании занятий атлета.
    /// </summary>
    public List<AthletePayerDto> Payers { get; set; }
    /// <summary>
    /// Список участия  атлета в  клубе 
    /// </summary>
    public List<AthleteMembershipDto> AthleteMemberships{ get; set; }
}

/// <summary>
/// DTO для представления плательщика атлета.
/// Используется для передачи данных о плательщике через API.
/// </summary>
public record AthletePayerDto
{
    /// <summary>
    /// Уникальный идентификатор плательщика.
    /// Используется для связи с конкретным плательщиком в системе.
    /// </summary>
    public Guid PayerId { get; init; }

    /// <summary>
    /// Имя плательщика.
    /// Должно быть непустой строкой и содержать корректное имя.
    /// Используется для идентификации плательщика.
    /// </summary>
    public string FirstName { get; init; }

    /// <summary>
    /// Фамилия плательщика.
    /// Должна быть непустой строкой и содержать корректную фамилию.
    /// Используется для идентификации плательщика.
    /// </summary>
    public string LastName { get; init; }

    /// <summary>
    /// Отчество плательщика.
    /// Может быть пустой строкой (например, для иностранных граждан или лиц без отчества).
    /// Используется для более точной идентификации плательщика.
    /// </summary>
    public string SecondName { get; init; }

    /// <summary>
    /// Описание типа связи плательщика с атлетом.
    /// Возможные значения: "Мама", "Папа", "Сам атлет", "Другое".
    /// Используется для отображения роли плательщика в интерфейсе.
    /// </summary>
    public string PayerTypeDescription { get; init; }
    public string PayerType { get; init; }
}
public static class AthletePayerDtoHelper
{
   public  static AthletePayerDto ToAthletePayerDto(this Domain.Payments.Payer payer, string payerTypeDescription, PayerType payerType)
    => new AthletePayerDto() {PayerId= payer.Id, FirstName= payer.FirstName, LastName =payer.LastName, SecondName= payer.SecondName, PayerTypeDescription= payerTypeDescription ,PayerType=payerType.ToString()};
    public static AthletePayerDto ToAthletePayerDto(this AthletePayer ap) => ap.Payer.ToAthletePayerDto($@"{ap.PayerType.ToString()} ( {ap.Payer.FirstName} {ap.Payer.LastName[0]}) ",ap.PayerType);
}