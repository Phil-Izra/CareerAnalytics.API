using System.Security.Claims;
using CareerAnalytics.Application.Applications.Commands.DetectGaps;
using CareerAnalytics.Application.Applications.Commands.ManageApplication;
using CareerAnalytics.Application.Applications.Commands.ScheduleInterview;
using CareerAnalytics.Application.Applications.Commands.ScoreCandidate;
using CareerAnalytics.Application.Applications.Commands.SubmitApplication;
using CareerAnalytics.Application.Applications.Queries.GetApplicationsByVacancy;
using CareerAnalytics.Application.Applications.Queries.GetApplicationWithScores;
using CareerAnalytics.Application.Applications.Queries.GetCandidateLeaderboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerAnalytics.API.Controllers;

[ApiController]
[Route("api/applications")]
[Authorize]
public sealed class ApplicationsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Submit(
        [FromBody] SubmitApplicationRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(
            new SubmitApplicationCommand(request.VacancyId, userId), cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.ErrorCode, result.Error });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new GetApplicationWithScoresQuery(id, userId), cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "Application.NotFound") return NotFound();
            if (result.ErrorCode == "Application.Forbidden") return Forbid();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return Ok(result.Value);
    }

    [HttpGet("by-vacancy/{vacancyId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByVacancy(
        [FromRoute] Guid vacancyId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(
            new GetApplicationsByVacancyQuery(vacancyId, userId), cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "Vacancy.NotFound") return NotFound();
            if (result.ErrorCode == "Vacancy.Forbidden") return Forbid();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return Ok(result.Value);
    }

    [HttpGet("by-vacancy/{vacancyId:guid}/leaderboard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLeaderboard(
        [FromRoute] Guid vacancyId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(
            new GetCandidateLeaderboardQuery(vacancyId, userId), cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "Vacancy.NotFound") return NotFound();
            if (result.ErrorCode == "Vacancy.Forbidden") return Forbid();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/score")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Score([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new ScoreCandidateCommand(id, userId), cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "Application.NotFound") return NotFound();
            if (result.ErrorCode == "Application.Forbidden") return Forbid();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return Ok(new { overallScore = result.Value });
    }

    [HttpPost("{id:guid}/detect-gaps")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DetectGaps([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new DetectGapsCommand(id, userId), cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "Application.NotFound") return NotFound();
            if (result.ErrorCode == "Application.Forbidden") return Forbid();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/schedule-interview")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ScheduleInterview(
        [FromRoute] Guid id,
        [FromBody] ScheduleInterviewRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(
            new ScheduleInterviewCommand(id, userId, request.ScheduledAt, request.IsRecorded),
            cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "Application.NotFound") return NotFound();
            if (result.ErrorCode == "Application.Forbidden") return Forbid();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return CreatedAtAction(nameof(GetById), new { id }, new { interviewId = result.Value });
    }

    [HttpPost("{id:guid}/shortlist")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Shortlist([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new ShortlistApplicationCommand(id, userId), cancellationToken);
        return ToStatusResult(result);
    }

    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new RejectApplicationCommand(id, userId), cancellationToken);
        return ToStatusResult(result);
    }

    [HttpPost("{id:guid}/offer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MakeOffer([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new MakeOfferCommand(id, userId), cancellationToken);
        return ToStatusResult(result);
    }

    [HttpPost("{id:guid}/withdraw")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Withdraw([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new WithdrawApplicationCommand(id, userId), cancellationToken);
        return ToStatusResult(result);
    }

    private IActionResult ToStatusResult(Application.Common.Models.Result result)
    {
        if (result.IsSuccess) return NoContent();
        if (result.ErrorCode?.EndsWith(".NotFound") == true) return NotFound();
        if (result.ErrorCode?.EndsWith(".Forbidden") == true) return Forbid();
        return BadRequest(new { result.ErrorCode, result.Error });
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new InvalidOperationException("UserId claim missing."));
}

public sealed record SubmitApplicationRequest(Guid VacancyId);
public sealed record ScheduleInterviewRequest(DateTime ScheduledAt, bool IsRecorded = false);
