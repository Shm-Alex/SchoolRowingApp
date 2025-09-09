// Application/Membership/Commands/InitializeMembershipPeriodsCommand.cs
using MediatR;
using SchoolRowingApp.Domain.Membership;
using SchoolRowingApp.Domain.SharedKernel;

namespace SchoolRowingApp.Application.Membership.Commands;

/// <summary>
/// Команда для инициализации периодов членства на указанный диапазон дат.
/// Используется администратором для заполнения календаря на будущие периоды.
/// </summary>
public record InitializeMembershipPeriodsCommand(
    DateTime StartDate,
    DateTime EndDate,
    decimal InitialBaseFee) : IRequest;

/// <summary>
/// Обработчик команды инициализации периодов членства.
/// Создает записи для всех месяцев в указанном диапазоне с базовым взносом.
/// </summary>
public class InitializeMembershipPeriodsCommandHandler :
    IRequestHandler<InitializeMembershipPeriodsCommand>
{
    private readonly IMembershipPeriodRepository _membershipPeriodRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InitializeMembershipPeriodsCommandHandler(
        IMembershipPeriodRepository membershipPeriodRepository,
        IUnitOfWork unitOfWork)
    {
        _membershipPeriodRepository = membershipPeriodRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        InitializeMembershipPeriodsCommand request,
        CancellationToken ct)
    {
        // Проверяем, что дата начала раньше даты окончания
        if (request.StartDate > request.EndDate)
            throw new Exception("Дата начала должна быть раньше даты окончания");

        // Очищаем существующие периоды в указанном диапазоне
        var existingPeriods = await _membershipPeriodRepository.GetAllAsync(ct);
        foreach (var period in existingPeriods)
        {
            if (period.StartDate >= request.StartDate && period.EndDate <= request.EndDate)
            {
                await _membershipPeriodRepository.DeleteAsync(period, ct);
            }
        }

        // Создаем новые периоды
        var current = new DateTime(request.StartDate.Year, request.StartDate.Month, 1);
        while (current <= request.EndDate)
        {
            var period = new MembershipPeriod(
                current.Month,
                current.Year,
                request.InitialBaseFee);

            await _membershipPeriodRepository.AddAsync(period, ct);
            current = current.AddMonths(1);
        }

        await _unitOfWork.SaveChangesAsync(ct);
    }
}