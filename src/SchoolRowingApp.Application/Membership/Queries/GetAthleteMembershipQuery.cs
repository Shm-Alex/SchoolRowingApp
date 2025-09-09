// Application/Membership/Queries/GetAthleteMembershipQuery.cs
using MediatR;
using SchoolRowingApp.Domain.Membership;
using System.Collections.Generic;

namespace SchoolRowingApp.Application.Membership.Queries;

/// <summary>
/// Запрос на получение членства атлета.
/// Используется для отображения истории членства атлета в UI.
/// </summary>
public record GetAthleteMembershipQuery(Guid AthleteId) : IRequest<List<AthleteMembership>>;

/// <summary>
/// Обработчик запроса на получение членства атлета.
/// Возвращает все записи о членстве атлета, включая информацию о периодах.
/// </summary>
public class GetAthleteMembershipQueryHandler :
    IRequestHandler<GetAthleteMembershipQuery, List<AthleteMembership>>
{
    private readonly IAthleteMembershipRepository _athleteMembershipRepository;

    public GetAthleteMembershipQueryHandler(
        IAthleteMembershipRepository athleteMembershipRepository)
    {
        _athleteMembershipRepository = athleteMembershipRepository;
    }

    public async Task<List<AthleteMembership>> Handle(
        GetAthleteMembershipQuery request,
        CancellationToken ct)
    {
        return await _athleteMembershipRepository.GetByAthleteIdAsync(request.AthleteId, ct);
    }
}