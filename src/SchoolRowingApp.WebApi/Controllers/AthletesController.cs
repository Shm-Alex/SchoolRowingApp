using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolRowingApp.Application.Athletes.Dto; 
using SchoolRowingApp.Application.Athletes.Queries.GetAthletes;

namespace SchoolRowingApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AthletesController : ControllerBase
{
    private readonly ISender _mediator;

    public AthletesController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Получить список всех атлетов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AthleteDto>>> Get(CancellationToken cancellationToken)
    {
        var athletes = await _mediator.Send(new GetAthletesQuery(), cancellationToken);
        return Ok(athletes);
    }
}