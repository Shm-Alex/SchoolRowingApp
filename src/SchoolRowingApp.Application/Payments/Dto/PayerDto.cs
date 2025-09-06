using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolRowingApp.Application.Payments.Dto
{
    // Application/Payments/DTOs/PayerDto.cs
    public record PayerDto(
        Guid Id,
        string FirstName,
        string SecondName,
        string LastName,
        string PaymentMatchingName,
        List<AthletePayerDto> AthletePayers);

    /// <summary>
    /// DTO, представляющий подопечного плательщика (атлета, за которого платит данный плательщик)
    /// </summary>
    public record AthletePayerDto(
        Guid AthleteId,
        string AthleteFullName,
        string PayerTypeDescription);
}
