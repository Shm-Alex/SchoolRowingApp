// WebApi/Controllers/MembershipController.cs
using Microsoft.AspNetCore.Mvc;
using MediatR;
using SchoolRowingApp.Application.Membership.Commands;
using SchoolRowingApp.Application.Membership.Queries;
using SchoolRowingApp.Domain.Membership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRowingApp.WebApi.Controllers;

/// <summary>
/// Контроллер для управления членством атлетов в школе.
/// Предоставляет API для работы с периодами членства и коэффициентами участия.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MembershipController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembershipController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получает все периоды членства, отсортированные по дате.
    /// Используется для отображения календаря членства в UI.
    /// </summary>
    [HttpGet("periods")]
    public async Task<ActionResult<List<MembershipPeriod>>> GetPeriods()
    {
        var periods = await _mediator.Send(new GetMembershipPeriodsQuery());
        return Ok(periods);
    }

    /// <summary>
    /// Получает членство атлета.
    /// Возвращает историю участия атлета в школе по месяцам.
    /// </summary>
    /// <param name="athleteId">ID атлета</param>
    [HttpGet("athletes/{athleteId}")]
    public async Task<ActionResult<List<AthleteMembership>>> GetAthleteMembership(Guid athleteId)
    {
        var memberships = await _mediator.Send(new GetAthleteMembershipQuery(athleteId));
        return Ok(memberships);
    }

    /// <summary>
    /// Устанавливает членство атлета на указанный период.
    /// Используется администратором через UI для фиксации факта членства.
    /// </summary>
    /// <remarks>
    /// Пример тела запроса:
    /// {
    ///   "athleteId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///   "membershipPeriodId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
    ///   "participationCoefficient": 1
    /// }
    /// </remarks>
    [HttpPost("athletes/membership")]
    public async Task<IActionResult> SetMembership([FromBody] SetAthleteMembershipCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Инициализирует периоды членства на указанный диапазон дат.
    /// Используется администратором для заполнения календаря на будущие периоды.
    /// </summary>
    /// <remarks>
    /// Пример тела запроса:
    /// {
    ///   "startDate": "2024-01-01T00:00:00",
    ///   "endDate": "2024-12-31T00:00:00",
    ///   "initialBaseFee": 2000
    /// }
    /// </remarks>
    [HttpPost("periods/initialize")]
    public async Task<IActionResult> InitializePeriods([FromBody] InitializeMembershipPeriodsCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Обновляет базовый взнос для всех будущих периодов.
    /// Используется при изменении стоимости членства (например, с 2000 на 3000 рублей).
    /// </summary>
    /// <remarks>
    /// Пример тела запроса:
    /// {
    ///   "newBaseFee": 3000
    /// }
    /// </remarks>
    [HttpPost("periods/update-base-fee")]
    public async Task<IActionResult> UpdateBaseFee([FromBody] UpdateFutureBaseFeesCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Удаляет членство атлета на указанный период.
    /// Используется при удалении записи через UI (например, при ошибке ввода).
    /// </summary>
    /// <param name="athleteId">ID атлета</param>
    /// <param name="periodId">ID периода членства</param>
    [HttpDelete("athletes/{athleteId}/membership/{periodId}")]
    public async Task<IActionResult> RemoveMembership(
        Guid athleteId,
        Guid periodId)
    {
        await _mediator.Send(new RemoveAthleteMembershipCommand(athleteId, periodId));
        return NoContent();
    }
}