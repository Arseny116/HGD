using System.Net;
using FluentAssertions;
using MiroIntegration;

namespace MiroIntegration.IntegrationTests;

public sealed class ProjectsApiTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProjectsApiTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProjects_ShouldReturnOk_ForAuthenticatedTestUser()
    {
        var response = await _client.GetAsync("/api/projects");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
