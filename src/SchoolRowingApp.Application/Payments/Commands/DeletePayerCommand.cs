using MediatR;
using SchoolRowingApp.Domain.Payments;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Payments.Commands;

public record DeletePayerCommand(Guid Id) : IRequest;

public class DeletePayerCommandHandler :
    IRequestHandler<DeletePayerCommand>
{
    private readonly IPayerRepository _payerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePayerCommandHandler(
        IPayerRepository payerRepository,
        IUnitOfWork unitOfWork)
    {
        _payerRepository = payerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeletePayerCommand request,
        CancellationToken ct)
    {
        var payer = await _payerRepository.GetByIdAsync(request.Id, ct);
        if (payer == null)
            throw new Exception("Плательщик не найден");

        await _payerRepository.DeleteAsync(payer, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}