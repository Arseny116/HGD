using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiroIntegration.Api.Controllers;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Application.Projects.DTOs;

namespace MiroIntegration.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public sealed class ProjectsController(
    IProjectService projectService) : ApiControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProject(
        Guid id,
        CancellationToken ct)
    {
        var project = await projectService.GetProjectAsync(id, ct);

        if (project is null)
            return NotFound();

        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(
        [FromBody] CreateProjectRequest request,
        CancellationToken ct)
    {
        var userId =base.CurrentUserId;

        var result = await projectService.CreateProjectAsync(
            userId,
            request,
            ct);

        if (result.IsFailure)
            return BadRequest(new
            {
                error = result.Error
            });

        var projectId = result.Value;

        return CreatedAtAction(
            nameof(GetProject),
            new { id = projectId },
            new { id = projectId });
    }
}