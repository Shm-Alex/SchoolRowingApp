// Application/Membership/Commands/UpdateFutureBaseFeesCommand.cs
using MediatR;
using SchoolRowingApp.Domain.Membership;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Membership.Commands;

/// <summary>
/// Команда для обновления базового взноса для всех будущих периодов.
/// Используется при изменении стоимости членства (например, с 2000 на 3000 рублей).
/// </summary>
public record UpdateFutureBaseFeesCommand(decimal NewBaseFee) : IRequest;

/// <summary>
/// Обработчик команды обновления базового взноса для будущих периодов.
/// Обновляет базовый взнос для всех периодов, начиная с текущего месяца.
/// </summary>
public class UpdateFutureBaseFeesCommandHandler :
    IRequestHandler<UpdateFutureBaseFeesCommand>
{
    private readonly IMembershipPeriodRepository _membershipPeriodRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFutureBaseFeesCommandHandler(
        IMembershipPeriodRepository membershipPeriodRepository,
        IUnitOfWork unitOfWork)
    {
        _membershipPeriodRepository = membershipPeriodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateFutureBaseFeesCommand request,
        CancellationToken ct)
    {
        var currentMonth = DateTime.UtcNow.Month;
        var currentYear = DateTime.UtcNow.Year;

        var periods = await _membershipPeriodRepository.GetAllAsync(ct);

        foreach (var period in periods)
        {
            // Обновляем только будущие периоды (включая текущий месяц)
            if (period.Year > currentYear ||
               (period.Year == currentYear && period.Month >= currentMonth))
            {
                period.UpdateBaseFee(request.NewBaseFee);
                await _membershipPeriodRepository.UpdateAsync(period, ct);
            }
        }

        await _unitOfWork.SaveChangesAsync(ct);
    }
}