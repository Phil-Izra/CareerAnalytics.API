using CareerAnalytics.Application.Users.Commands.Login;
using CareerAnalytics.Application.Users.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CareerAnalytics.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType<RegisterResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.ErrorCode == "User.EmailTaken")
                return Conflict(new { result.ErrorCode, result.Error });

            return BadRequest(new { result.ErrorCode, result.Error });
        }

        return CreatedAtAction(nameof(Register), result.Value);
    }

    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return Unauthorized(new { result.ErrorCode, result.Error });

        return Ok(result.Value);
    }
}
