using Amazon.S3;
using MassTransit;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiroIntegration.Application.Abstractions;
using Shared.Contracts;
using static Shared.Contracts.Contracts;



namespace MiroIntegration.Api.Controllers;

[Route("api/v1/pdfs")]
[Authorize]
public sealed class PdfGeneratorController : ApiControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IPdfService _pdfService;
    private readonly IMapper _mapper;
    private readonly ISendEndpointProvider _sendEndpoint;
    private readonly IPdfFileStorage _fileStorage;

    public PdfGeneratorController(
        IProjectService projectService,
        IPdfService pdfService,
        IMapper mapper,
        ISendEndpointProvider sendEndpoint,
        IPdfFileStorage fileStorage)
    {
        _projectService = projectService;
        _pdfService = pdfService;
        _mapper = mapper;
        _sendEndpoint = sendEndpoint;
        _fileStorage = fileStorage;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] PdfGenerationStartRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _projectService.GetProjectAsync(
            request.ProjectId,
            CurrentUserId,
            cancellationToken);

        if (project is null)
            return NotFound();

        var pdfResult = await _pdfService.CreateAsync(
            CurrentUserId,
            request.ProjectId,
            request.FileName,
            cancellationToken);

        if (pdfResult.IsFailure)
            return BadRequest(new { error = pdfResult.Error });

        var pdf = pdfResult.Value;
        var endpoint = await _sendEndpoint.GetSendEndpoint(
            new Uri("queue:pdf-generation-queue"));

        try
        {
            await endpoint.Send(
                new PdfGenerationRequest(
                    CurrentUserId,
                    pdf.JobId,
                    project.Id,
                    pdf.FileName,
                    _mapper.Map<ProjectPdfData>(project)),
                cancellationToken);
        }
        catch
        {
            await _pdfService.FailAsync(
                pdf.JobId,
                CurrentUserId,
                cancellationToken);
            throw;
        }

        return Accepted(ToDto(pdf));
    }

    [HttpGet]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var pdfs = await _pdfService.GetByUserIdAsync(
            CurrentUserId,
            cancellationToken);

        return Ok(pdfs.Select(ToDto));
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(
        Guid id,
        CancellationToken cancellationToken)
    {
        var pdf = await _pdfService.GetByIdAsync(
            id,
            CurrentUserId,
            cancellationToken);

        if (pdf is null)
            return NotFound();

        if (pdf.Status != MiroIntegration.Domain.Models.Pdf.State.Completed ||
            string.IsNullOrWhiteSpace(pdf.ObjectKey))
        {
            return Conflict(new
            {
                error = "PDF is not ready yet.",
                status = pdf.Status.ToString()
            });
        }

        try
        {
            var stream = await _fileStorage.OpenReadAsync(
                pdf.ObjectKey,
                cancellationToken);

            return File(
                stream,
                "application/pdf",
                pdf.FileName,
                enableRangeProcessing: true);
        }
        catch (AmazonS3Exception exception) when (
            exception.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return NotFound();
        }
    }

    private static PdfDto ToDto(MiroIntegration.Domain.Models.Pdf pdf) => new(
        pdf.Id,
        pdf.JobId,
        pdf.ProjectId,
        pdf.FileName,
        pdf.Status.ToString(),
        string.IsNullOrWhiteSpace(pdf.FileUrl) ? null : pdf.FileUrl);
}

[ApiController]
[Route("api/v1/projects/{projectId:guid}/pdfs")]
[Authorize]
public sealed class ProjectPdfsController(
    IPdfService pdfService,
    IProjectService projectService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProjectPdfs(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var project = await projectService.GetProjectAsync(
            projectId,
            CurrentUserId,
            cancellationToken);

        if (project is null)
            return NotFound();

        var pdfs = await pdfService.GetByProjectIdAsync(
            projectId,
            CurrentUserId,
            cancellationToken);

        return Ok(pdfs.Select(pdf => new PdfDto(
            pdf.Id,
            pdf.JobId,
            pdf.ProjectId,
            pdf.FileName,
            pdf.Status.ToString(),
            string.IsNullOrWhiteSpace(pdf.FileUrl) ? null : pdf.FileUrl)));
    }
}
