// WebApi/Controllers/MembershipController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolRowingApp.Application.Membership.Commands;
using SchoolRowingApp.Application.Membership.Dto;
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
    private readonly ILogger _logger;

    public MembershipController(IMediator mediator,
        ILogger<MembershipController> logger

        )
    {
        _mediator = mediator;
        _logger= logger;
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
    /// <summary>
    /// Создает отсутствующие периоды членства в указанном диапазоне.
    /// Создает новые периоды только для тех месяцев, которые отсутствуют в базе данных.
    /// Пример: создание периодов с 1 сентября 2024 по 31 декабря 2024.
    /// </summary>
    [HttpPost("periods/missing")]
    [ProducesResponseType(typeof(List<MembershipPeriodDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<MembershipPeriodDto>>> CreateMissingPeriods(
        [FromBody] CreateMissingPeriodsCommand command)
    {
        //var validationResult = await _validator.ValidateAsync(command);
        //if (!validationResult.IsValid)
        //{
        //    return BadRequest(validationResult.Errors);
        //}

        var createdPeriods = await _mediator.Send(command);

        // Сортируем периоды по дате
        var sortedPeriods = createdPeriods
            .OrderBy(p => p.Year)
            .ThenBy(p => p.Month)
            .ToList();

        _logger.LogInformation("Создано {Count} новых периодов", sortedPeriods.Count);

        return Ok(sortedPeriods);
    }
    /// <summary>
    /// Обновляет базовый взнос для существующих периодов членства в указанном диапазоне.
    /// Обновляет только те периоды, которые уже существуют в базе данных.
    /// Пример: обновление базового взноса для периодов с 1 января 2024 по 31 августа 2024.
    /// </summary>
    [HttpPut("periods/existing")]
    [ProducesResponseType(typeof(List<MembershipPeriodDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<MembershipPeriodDto>>> UpdateExistingPeriods(
        [FromBody] UpdateExistingPeriodsCommand command)
    {
        var updatedPeriods = await _mediator.Send(command);

        if (!updatedPeriods.Any())
        {
            return NotFound(new { message = "Периоды для обновления не найдены" });
        }

        // Сортируем периоды по дате
        var sortedPeriods = updatedPeriods
            .OrderBy(p => p.Year)
            .ThenBy(p => p.Month)
            .ToList();

        _logger.LogInformation("Обновлено {Count} существующих периодов", sortedPeriods.Count);

        return Ok(sortedPeriods);
    }

}