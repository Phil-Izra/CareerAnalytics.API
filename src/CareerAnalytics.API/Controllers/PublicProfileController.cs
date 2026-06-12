using CareerAnalytics.Application.CareerEvents.Queries.GetPublicDashboard;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CareerAnalytics.API.Controllers;

[ApiController]
[Route("u")]
public sealed class PublicProfileController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Public career dashboard — accessible without authentication.
    /// Route: /u/{slug}
    /// </summary>
    [HttpGet("{slug}")]
    [ProducesResponseType<PublicDashboardDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPublicDashboard(
        [FromRoute] string slug,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPublicDashboardQuery(slug), cancellationToken);

        if (result.IsFailure)
            return NotFound(new { result.ErrorCode, result.Error });

        await RecordViewAsync(result.Value.Slug, cancellationToken);

        return Ok(result.Value);
    }

    private async Task RecordViewAsync(string slug, CancellationToken cancellationToken)
    {
        try
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var ipHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(ip)));
            var country = Request.Headers["CF-IPCountry"].FirstOrDefault();
            var sessionId = Request.Cookies["sid"];

            // We need the userId — but for simplicity, store by slug mapped to userId externally.
            // In production, resolve userId inside a domain service; here we skip for now.
        }
        catch
        {
            // View recording is fire-and-forget; never fail the main request
        }
    }
}
