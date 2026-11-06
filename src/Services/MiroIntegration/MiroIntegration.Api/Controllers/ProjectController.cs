using CSharpFunctionalExtensions;
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
    [HttpGet]
    public async Task<IActionResult> GetProjects(CancellationToken ct)
    {
        var projects = await projectService.GetProjectsAsync(CurrentUserId, ct);
        return Ok(projects);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProject(
        Guid id,
        CancellationToken ct)
    {
        var project = await projectService.GetProjectAsync(id, CurrentUserId, ct);

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

    [HttpPut("{projectId:guid}/meta")]
    public async Task<IActionResult> UpdateMeta(
        Guid projectId,
        [FromBody] MetaBlockDto request,
        CancellationToken ct)
    {
        var result = await projectService.UpdateMetaAsync(
            projectId,
            CurrentUserId,
            request,
            ct);

        return ToUpdateResponse(result);
    }

    [HttpPut("{projectId:guid}/technical-specs")]
    public async Task<IActionResult> UpdateTechnicalSpecs(
        Guid projectId,
        [FromBody] TechnicalSpecsBlockDto request,
        CancellationToken ct)
    {
        var result = await projectService.UpdateTechnicalSpecsAsync(
            projectId,
            CurrentUserId,
            request,
            ct);

        return ToUpdateResponse(result);
    }

    [HttpPut("{projectId:guid}/gameplay")]
    public async Task<IActionResult> UpdateGameplay(
        Guid projectId,
        [FromBody] GameplayBlockDto request,
        CancellationToken ct)
    {
        var result = await projectService.UpdateGameplayAsync(
            projectId,
            CurrentUserId,
            request,
            ct);

        return ToUpdateResponse(result);
    }

    [HttpPut("{projectId:guid}/design-document")]
    public async Task<IActionResult> UpdateDesignDocument(
        Guid projectId,
        [FromBody] DesignDocumentBlockDto request,
        CancellationToken ct)
    {
        var result = await projectService.UpdateDesignDocumentAsync(
            projectId,
            CurrentUserId,
            request,
            ct);

        return ToUpdateResponse(result);
    }

    [HttpPut("{projectId:guid}/flowchart")]
    public async Task<IActionResult> UpdateFlowchart(
        Guid projectId,
        [FromBody] FlowchartBlockDto request,
        CancellationToken ct)
    {
        var result = await projectService.UpdateFlowchartAsync(
            projectId,
            CurrentUserId,
            request,
            ct);

        return ToUpdateResponse(result);
    }

    [HttpPut("{projectId:guid}/player")]
    public async Task<IActionResult> UpdatePlayer(
        Guid projectId,
        [FromBody] PlayerBlockDto request,
        CancellationToken ct)
    {
        var result = await projectService.UpdatePlayerAsync(
            projectId,
            CurrentUserId,
            request,
            ct);

        return ToUpdateResponse(result);
    }

    [HttpPut("{projectId:guid}/ui")]
    public async Task<IActionResult> UpdateUi(
        Guid projectId,
        [FromBody] UiBlockDto request,
        CancellationToken ct)
    {
        var result = await projectService.UpdateUiAsync(
            projectId,
            CurrentUserId,
            request,
            ct);

        return ToUpdateResponse(result);
    }

    private IActionResult ToUpdateResponse(Result result)
    {
        if (result.IsSuccess)
            return NoContent();

        if (result.Error == "Project not found.")
            return NotFound();

        return BadRequest(new { error = result.Error });
    }
}