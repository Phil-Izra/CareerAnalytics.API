using System.Security.Claims;
using CareerAnalytics.Application.Vacancies.Commands.CloseVacancy;
using CareerAnalytics.Application.Vacancies.Commands.ConfigureVacancyScoring;
using CareerAnalytics.Application.Vacancies.Commands.CreateVacancy;
using CareerAnalytics.Application.Vacancies.Commands.OpenVacancy;
using CareerAnalytics.Application.Vacancies.Queries.GetOpenVacancies;
using CareerAnalytics.Application.Vacancies.Queries.GetVacanciesByRecruiter;
using CareerAnalytics.Application.Vacancies.Queries.GetVacancyById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerAnalytics.API.Controllers;

[ApiController]
[Route("api/vacancies")]
[Authorize]
public sealed class VacanciesController(IMediator mediator) : ControllerBase
{
    [HttpGet("open")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOpen(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOpenVacanciesQuery(), cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("my")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new GetVacanciesByRecruiterQuery(userId), cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVacancyByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "Vacancy.NotFound") return NotFound();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateVacancyCommand command, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(command with { RecruiterUserId = userId }, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.ErrorCode, result.Error });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
    }

    [HttpPut("{id:guid}/scoring")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfigureScoring(
        [FromRoute] Guid id,
        [FromBody] ConfigureVacancyScoringCommand command,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(
            command with { VacancyId = id, RecruiterUserId = userId }, cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "Vacancy.NotFound") return NotFound();
            if (result.ErrorCode == "Vacancy.Forbidden") return Forbid();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return NoContent();
    }

    [HttpPost("{id:guid}/open")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Open([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new OpenVacancyCommand(id, userId), cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "Vacancy.NotFound") return NotFound();
            if (result.ErrorCode == "Vacancy.Forbidden") return Forbid();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return NoContent();
    }

    [HttpPost("{id:guid}/close")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Close([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new CloseVacancyCommand(id, userId), cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "Vacancy.NotFound") return NotFound();
            if (result.ErrorCode == "Vacancy.Forbidden") return Forbid();
            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return NoContent();
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new InvalidOperationException("UserId claim missing."));
}
