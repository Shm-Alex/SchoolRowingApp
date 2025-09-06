using SchoolRowingApp.Application.Payments.Dto;
using SchoolRowingApp.Domain.Payments;

namespace SchoolRowingApp.Application.Payments.Queries;

public record GetPayerQuery(Guid Id) : IRequest<PayerDto>;

public class GetPayerQueryHandler :
    IRequestHandler<GetPayerQuery, PayerDto>
{
    private readonly IPayerRepository _PayerRepository;

    public GetPayerQueryHandler(IPayerRepository PayerRepository)
    {
        _PayerRepository = PayerRepository;
    }

    public async Task<PayerDto> Handle(
        GetPayerQuery request,
        CancellationToken ct)
    {
        var Payer = await _PayerRepository.GetByIdAsync(request.Id, ct);
        if (Payer == null)
            throw new Exception("Атлет не найден");

        
        return new PayerDto(
            Payer.Id,
            Payer.FirstName, 
            Payer.SecondName,
            Payer.LastName,
            @$"{Payer.FirstName} {Payer.LastName[0]}.",
            Payer.AthletePayers.Select(
                ap => new AthletePayerDto
                (
                    ap.AthleteId,
                    $@"{ap.Athlete.FirstName} {ap.Athlete.SecondName} {ap.Athlete.LastName}",
                    $@"{ap.PayerType.ToString()} ( {ap.Athlete.FirstName} {ap.Athlete.LastName[0]}. )" 
                    )
                ).ToList()

            );
    }
}