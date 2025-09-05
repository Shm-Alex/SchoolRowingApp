using MediatR;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Payments;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Athletes.Commands;

public record AddPayerToAthleteCommand(
    Guid AthleteId,
    Guid PayerId,
    PayerType PayerType) : IRequest;

public class AddPayerToAthleteCommandHandler :
    IRequestHandler<AddPayerToAthleteCommand>
{
    private readonly IAthleteRepository _athleteRepository;
    private readonly IPayerRepository _payerRepository;
    private readonly IAthletePayerRepository _athletePayerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPayerToAthleteCommandHandler(
        IAthleteRepository athleteRepository,
        IPayerRepository payerRepository,
        IAthletePayerRepository athletePayerRepository,
        IUnitOfWork unitOfWork)
    {
        _athleteRepository = athleteRepository;
        _payerRepository = payerRepository;
        _athletePayerRepository = athletePayerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        AddPayerToAthleteCommand request,
        CancellationToken ct)
    {
        var athlete = await _athleteRepository.GetByIdAsync(request.AthleteId, ct);
        if (athlete == null)
            throw new Exception("Атлет не найден");

        var payer = await _payerRepository.GetByIdAsync(request.PayerId, ct);
        if (payer == null)
            throw new Exception("Плательщик не найден");

        athlete.AddPayer(request.PayerId, request.PayerType);

        await _athleteRepository.UpdateAsync(athlete, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}