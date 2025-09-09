// Application/Membership/Queries/GetMembershipPeriodsQuery.cs
using MediatR;
using SchoolRowingApp.Domain.Membership;
using System.Collections.Generic;

namespace SchoolRowingApp.Application.Membership.Queries;

/// <summary>
/// Запрос на получение всех периодов членства.
/// Используется для отображения календаря членства в UI.
/// </summary>
public record GetMembershipPeriodsQuery : IRequest<List<MembershipPeriod>>;

/// <summary>
/// Обработчик запроса на получение всех периодов членства.
/// Возвращает периоды, отсортированные по дате.
/// </summary>
public class GetMembershipPeriodsQueryHandler :
    IRequestHandler<GetMembershipPeriodsQuery, List<MembershipPeriod>>
{
    private readonly IMembershipPeriodRepository _membershipPeriodRepository;

    public GetMembershipPeriodsQueryHandler(
        IMembershipPeriodRepository membershipPeriodRepository)
    {
        _membershipPeriodRepository = membershipPeriodRepository;
    }

    public async Task<List<MembershipPeriod>> Handle(
        GetMembershipPeriodsQuery request,
        CancellationToken ct)
    {
        var periods = await _membershipPeriodRepository.GetAllAsync(ct);
        return periods.OrderBy(p => p.Year).ThenBy(p => p.Month).ToList();
    }
}