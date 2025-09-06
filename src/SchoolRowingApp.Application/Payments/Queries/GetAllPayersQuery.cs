using MediatR;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Payments;
using System.Collections.Generic;

namespace SchoolRowingApp.Application.Payments.Queries;

public record GetAllPayersQuery : IRequest<List<Payer>>;

public class GetAllPayersQueryHandler :
    IRequestHandler<GetAllPayersQuery, List<Payer>>
{
    private readonly IPayerRepository _payerRepository;

    public GetAllPayersQueryHandler(IPayerRepository payerRepository)
    {
        _payerRepository = payerRepository;
    }

    public async Task<List<Payer>> Handle(
        GetAllPayersQuery request,
        CancellationToken ct)
    {
        return await _payerRepository.GetAllAsync(ct);
    }
}
