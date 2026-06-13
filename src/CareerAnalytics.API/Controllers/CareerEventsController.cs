using System.Security.Claims;
using CareerAnalytics.Application.CareerEvents.Commands.AddAchievement;
using CareerAnalytics.Application.CareerEvents.Commands.CreateCareerEvent;
using CareerAnalytics.Application.CareerEvents.Commands.DeleteCareerEvent;
using CareerAnalytics.Application.CareerEvents.Queries.GetCareerEventById;
using CareerAnalytics.Application.CareerEvents.Queries.GetCareerTimeline;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerAnalytics.API.Controllers;

[ApiController]
[Route("api/career-events")]
[Authorize]
public sealed class CareerEventsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new GetCareerEventByIdQuery(id, userId), cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "CareerEvent.Forbidden") return Forbid();
            if (result.ErrorCode == "CareerEvent.NotFound") return NotFound();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTimeline(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(
            new GetCareerTimelineQuery(userId, PublicOnly: false), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { result.ErrorCode, result.Error });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCareerEventCommand command,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(command with { UserId = userId }, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.ErrorCode, result.Error });

        return CreatedAtAction(nameof(Create), new { id = result.Value }, new { id = result.Value });
    }

    [HttpPost("{id:guid}/achievements")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddAchievement(
        [FromRoute] Guid id,
        [FromBody] AddAchievementCommand command,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(
            command with { CareerEventId = id, RequestingUserId = userId }, cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "CareerEvent.Forbidden") return Forbid();
            if (result.ErrorCode == "CareerEvent.NotFound") return NotFound();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new DeleteCareerEventCommand(id, userId), cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "CareerEvent.Forbidden") return Forbid();
            if (result.ErrorCode == "CareerEvent.NotFound") return NotFound();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return NoContent();
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new InvalidOperationException("UserId claim missing."));
}
