using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiroIntegration.Application.Features.Projects;

namespace MiroIntegration.Api.Controllers;

[Route("api/v1/core_pillars")]
[Authorize]
public sealed class ProjectsController(ISender sender) : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create([FromBody] ProjectEnvelope request, CancellationToken cancellationToken)
    {
        var response = await sender.Send(new CreateProjectCommand(request.Project.Name, request.Project.Projects), cancellationToken);
        return Created($"/api/v1/core_pillars/{response.Id}", response);
    }
}

public sealed record ProjectEnvelope(ProjectPayload Project);
public sealed record ProjectPayload(string Name, [property: System.Text.Json.Serialization.JsonPropertyName("core_pillars")] JsonElement Projects);