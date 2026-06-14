using System.Security.Claims;
using CareerAnalytics.Application.CandidatePortfolio.Commands.CommunityInvolvement;
using CareerAnalytics.Application.CandidatePortfolio.Commands.Education;
using CareerAnalytics.Application.CandidatePortfolio.Commands.Hobbies;
using CareerAnalytics.Application.CandidatePortfolio.Commands.Languages;
using CareerAnalytics.Application.CandidatePortfolio.Queries.GetCommunityInvolvement;
using CareerAnalytics.Application.CandidatePortfolio.Queries.GetEducation;
using CareerAnalytics.Application.CandidatePortfolio.Queries.GetHobbies;
using CareerAnalytics.Application.CandidatePortfolio.Queries.GetLanguages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerAnalytics.API.Controllers;

[ApiController]
[Route("api/portfolio")]
[Authorize]
public sealed class CandidatePortfolioController(IMediator mediator) : ControllerBase
{
    // ── Education ──────────────────────────────────────────────────────────────────

    [HttpGet("education")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEducation(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new GetEducationByUserQuery(userId), cancellationToken);
        return Ok(result.Value);
    }

    [HttpPost("education")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddEducation(
        [FromBody] AddEducationCommand command,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(command with { UserId = userId }, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.ErrorCode, result.Error });

        return CreatedAtAction(nameof(GetEducation), new { id = result.Value }, new { id = result.Value });
    }

    [HttpDelete("education/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEducation([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new DeleteEducationCommand(id, userId), cancellationToken);
        return ToStatusResult(result);
    }

    // ── Languages ──────────────────────────────────────────────────────────────────

    [HttpGet("languages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLanguages(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new GetLanguagesByUserQuery(userId), cancellationToken);
        return Ok(result.Value);
    }

    [HttpPost("languages")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddLanguage(
        [FromBody] AddLanguageCommand command,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(command with { UserId = userId }, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.ErrorCode, result.Error });

        return CreatedAtAction(nameof(GetLanguages), new { id = result.Value }, new { id = result.Value });
    }

    [HttpPut("languages/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLanguage(
        [FromRoute] Guid id,
        [FromBody] UpdateLanguageProficiencyCommand command,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(
            command with { LanguageId = id, RequestingUserId = userId }, cancellationToken);
        return ToStatusResult(result);
    }

    [HttpDelete("languages/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLanguage([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new DeleteLanguageCommand(id, userId), cancellationToken);
        return ToStatusResult(result);
    }

    // ── Hobbies ────────────────────────────────────────────────────────────────────

    [HttpGet("hobbies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHobbies(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new GetHobbiesByUserQuery(userId), cancellationToken);
        return Ok(result.Value);
    }

    [HttpPost("hobbies")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddHobby(
        [FromBody] AddHobbyCommand command,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(command with { UserId = userId }, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.ErrorCode, result.Error });

        return CreatedAtAction(nameof(GetHobbies), new { id = result.Value }, new { id = result.Value });
    }

    [HttpPut("hobbies/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHobby(
        [FromRoute] Guid id,
        [FromBody] UpdateHobbyCommand command,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(
            command with { HobbyId = id, RequestingUserId = userId }, cancellationToken);
        return ToStatusResult(result);
    }

    [HttpDelete("hobbies/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHobby([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new DeleteHobbyCommand(id, userId), cancellationToken);
        return ToStatusResult(result);
    }

    // ── Community Involvement ──────────────────────────────────────────────────────

    [HttpGet("community")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCommunity(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new GetCommunityInvolvementByUserQuery(userId), cancellationToken);
        return Ok(result.Value);
    }

    [HttpPost("community")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddCommunity(
        [FromBody] AddCommunityInvolvementCommand command,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(command with { UserId = userId }, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.ErrorCode, result.Error });

        return CreatedAtAction(nameof(GetCommunity), new { id = result.Value }, new { id = result.Value });
    }

    [HttpDelete("community/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCommunity([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new DeleteCommunityInvolvementCommand(id, userId), cancellationToken);
        return ToStatusResult(result);
    }

    // ── Helpers ────────────────────────────────────────────────────────────────────

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
