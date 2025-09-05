using MediatR;
using SchoolRowingApp.Domain.Payments;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Payments.Commands;

public record CreatePayerCommand(
    string FirstName,
    string SecondName,
    string LastName) : IRequest<Guid>;

public class CreatePayerCommandHandler :
    IRequestHandler<CreatePayerCommand, Guid>
{
    private readonly IPayerRepository _payerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePayerCommandHandler(
        IPayerRepository payerRepository,
        IUnitOfWork unitOfWork)
    {
        _payerRepository = payerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreatePayerCommand request,
        CancellationToken ct)
    {
        // Проверяем уникальность через репозиторий
        var existingPayer = await _payerRepository.GetByFullNameAsync(
            request.FirstName,
            request.SecondName,
            request.LastName,
            ct);

        if (existingPayer != null)
            throw new Exception("Плательщик с таким ФИО уже существует");

        var payer = new Payer(
            request.FirstName,
            request.SecondName,
            request.LastName);

        await _payerRepository.AddAsync(payer, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return payer.Id;
    }
}