using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MiroIntegration.IntegrationTests;

public sealed class TestAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, TestWebApplicationFactory.TestUserId.ToString()),
            new Claim(ClaimTypes.Name, "integration-test-user")
        };

        var identity = new ClaimsIdentity(claims, TestWebApplicationFactory.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(
            principal,
            TestWebApplicationFactory.AuthenticationScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
