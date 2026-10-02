using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiroIntegration.Application.Features.Auth;

namespace MiroIntegration.Api.Controllers;

[Route("api/v1")]
public sealed class AuthController(ISender sender) : ApiControllerBase
{
    [HttpPost("users")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterEnvelope request, CancellationToken cancellationToken)
    {
        var response = await sender.Send(request.User, cancellationToken);
        SetAuthCookie(response.Token);
        return Created("/api/v1/me", response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginCommand request, CancellationToken cancellationToken)
    {
        var response = await sender.Send(request, cancellationToken);
        SetAuthCookie(response.Token);
        return Ok(response);
    }

    [HttpDelete("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("miro_auth");
        return NoContent();
    }

    [HttpGet("me")]
    public async Task<ActionResult<object>> Me(CancellationToken cancellationToken) => Ok(new { user = await sender.Send(new GetCurrentUserQuery(CurrentUserId), cancellationToken) });

    private void SetAuthCookie(string token) => Response.Cookies.Append("miro_auth", token, new CookieOptions { HttpOnly = true, Secure = !HttpContext.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment(), SameSite = SameSiteMode.Lax, Expires = DateTimeOffset.UtcNow.AddHours(24) });
}

public sealed record RegisterEnvelope(RegisterCommand User);