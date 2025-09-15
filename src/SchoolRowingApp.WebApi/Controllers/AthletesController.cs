using Microsoft.AspNetCore.Mvc;
using MediatR;
using SchoolRowingApp.Application.Athletes.Commands;
using SchoolRowingApp.Application.Athletes.Queries;
using SchoolRowingApp.Domain.Athletes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRowingApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AthletesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AthletesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<Athlete>>> GetAll()
    {
        var athletes = await _mediator.Send(new GetAllAthletesQuery());
        return Ok(athletes);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Athlete>> GetById(Guid id)
    {
        var athlete = await _mediator.Send(new GetAthleteQuery(id));
        if (athlete == null)
        {
            return NotFound(new { message = "Атлет не найден" }); // HTTP 404
        }
        return Ok(athlete);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateAthleteCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateAthleteCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteAthleteCommand(id));
        return NoContent();
    }
}