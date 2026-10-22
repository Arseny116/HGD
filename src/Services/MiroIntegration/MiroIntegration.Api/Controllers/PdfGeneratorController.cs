using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiroIntegration.Application.Abstractions;
using static Shared.Contracts.Contracts;



namespace MiroIntegration.Api.Controllers;

[Route("api/v1/pdfs")]
[Authorize]
public sealed class PdfGeneratorController : ApiControllerBase
{

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromQuery] Guid id,
        [FromServices] ISendEndpointProvider sendEndpoint,
        [FromServices] IProjectService ps,
        CancellationToken cancellationToken)
    {
        var project = await ps.GetProjectAsync(id, cancellationToken);

        if (project is null)
        {
            return NotFound();
        }

        var taskId = Guid.NewGuid();

        var endpoint = await sendEndpoint.GetSendEndpoint(
            new Uri("queue:pdf-generation-queue"));

        await endpoint.Send(
            new PdfGenerationRequest(CurrentUserId,taskId,project.Name,project),
            cancellationToken);

        return Accepted(new
        {
            TaskId = taskId,
            ProjectId = id,
            Status = "Queued"
        });
    }
}
