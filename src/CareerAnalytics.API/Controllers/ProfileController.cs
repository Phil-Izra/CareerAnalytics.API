using System.Security.Claims;
using CareerAnalytics.Application.Profiles.Commands.UpdateProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerAnalytics.API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public sealed class ProfileController(IMediator mediator) : ControllerBase
{
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateProfileCommand command,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        var result = await mediator.Send(command with { UserId = userId }, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.ErrorCode, result.Error });

        return NoContent();
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new InvalidOperationException("UserId claim missing."));
}
