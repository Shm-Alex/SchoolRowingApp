using MediatR;
using Microsoft.AspNetCore.Mvc;
using SchoolRowingApp.Application.Athletes.Commands;
using SchoolRowingApp.Application.Athletes.Queries;
using SchoolRowingApp.Application.Payments.Commands;
using SchoolRowingApp.Application.Payments.Queries;
using SchoolRowingApp.Domain.Payments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolRowingApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayersController : ControllerBase
{
    private readonly IMediator _mediator;

    public PayersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<Payer>>> GetAll()
    {
        var payers = await _mediator.Send(new GetAllPayersQuery());
        return Ok(payers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Payer>> GetById(Guid id)
    {
   

        var payer = await _mediator.Send(new GetPayerQuery(id));
        return Ok(payer);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreatePayerCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePayerCommand command)
    {
        command = command with { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeletePayerCommand(id));
        return NoContent();
    }

    [HttpPost("{athleteId}/payers")]
    public async Task<IActionResult> AddPayerToAthlete(
        Guid athleteId,
        [FromBody] AddPayerToAthleteCommand command,
        CancellationToken ct)
    {
        command = command with { AthleteId = athleteId };
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{athleteId}/payers")]
    public async Task<IActionResult> RemovePayerFromAthlete(
        Guid athleteId,
        [FromBody] RemovePayerFromAthleteCommand command,
        CancellationToken ct)
    {
        command = command with { AthleteId = athleteId };
        await _mediator.Send(command, ct);
        return NoContent();
    }
}