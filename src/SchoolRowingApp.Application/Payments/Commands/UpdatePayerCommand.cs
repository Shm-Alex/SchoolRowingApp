using MediatR;
using SchoolRowingApp.Domain.Payments;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Payments.Commands;

public record UpdatePayerCommand(
    Guid Id,
    string FirstName,
    string SecondName,
    string LastName) : IRequest;

public class UpdatePayerCommandHandler :
    IRequestHandler<UpdatePayerCommand>
{
    private readonly IPayerRepository _payerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePayerCommandHandler(
        IPayerRepository payerRepository,
        IUnitOfWork unitOfWork)
    {
        _payerRepository = payerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdatePayerCommand request,
        CancellationToken ct)
    {
        var payer = await _payerRepository.GetByIdAsync(request.Id, ct);
        if (payer == null)
            throw new Exception("Плательщик не найден");

        // Проверяем уникальность (исключая текущего плательщика)
        var existingPayer = await _payerRepository.GetByFullNameAsync(
            request.FirstName,
            request.SecondName,
            request.LastName,
            ct);

        if (existingPayer != null && existingPayer.Id != request.Id)
            throw new Exception("Плательщик с таким ФИО уже существует");

        payer.UpdateName(
            request.FirstName,
            request.SecondName,
            request.LastName);

        await _payerRepository.UpdateAsync(payer, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}