using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiroIntegration.Application.Features.Pdf;

namespace MiroIntegration.Api.Controllers;

[Route("api/v1/pdf_generator")]
[Authorize]
public sealed class PdfGeneratorController(ISender sender) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PdfRequest request, CancellationToken cancellationToken)
    {
        await sender.Send(new QueuePdfGenerationCommand(request.StickerId), cancellationToken);
        return Accepted(new { message = "PDF generation job has been queued." });
    }
}

public sealed record PdfRequest([property: System.Text.Json.Serialization.JsonPropertyName("sticker_id")] Guid StickerId);