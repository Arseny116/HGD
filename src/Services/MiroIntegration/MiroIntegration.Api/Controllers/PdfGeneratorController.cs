using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace MiroIntegration.Api.Controllers;

[Route("api/v1/pdfs")]
[Authorize]
public sealed class PdfGeneratorController() : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PdfRequest request, CancellationToken cancellationToken)
    {
     
    }
}

