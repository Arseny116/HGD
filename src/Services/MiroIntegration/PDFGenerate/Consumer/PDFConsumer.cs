using MassTransit;
using MassTransit.Transports;
using PdfWorker.Services.Storage;
using static Shared.Contracts.Contracts;

namespace PdfWorker.Services;

public sealed class PdfGenerationConsumer : IConsumer<PdfGenerationRequest>
{
    private readonly PdfGenerator _pdfGenerator;
    private readonly S3FileStorage _fileStorage;

    private readonly ISendEndpointProvider _sendEndpoint;

    public PdfGenerationConsumer(ISendEndpointProvider SendEndpoint, PdfGenerator pdfGenerator, S3FileStorage fileStorage)
    {
        _sendEndpoint = SendEndpoint;
        _pdfGenerator = pdfGenerator;
        _fileStorage = fileStorage;
    }

    public async Task Consume(ConsumeContext<PdfGenerationRequest> context)
    {
        var request = context.Message;
        var endpoint = await _sendEndpoint.GetSendEndpoint(new Uri("queue:pdf-generation-queue-answer"));

        try
        {
            var pdfBytes = _pdfGenerator.Generate(request);
            var objectKey = $"{request.JobId}.pdf";

            await _fileStorage.UploadAsync(
                pdfBytes,
                objectKey,
                context.CancellationToken);

            var downloadUrl = _fileStorage.GetDownloadUrl(objectKey);

            await endpoint.Send(new PdfGenerationCompleted(
                request.JobId,
                request.ProjectId,
                request.UserId,
                objectKey,
                downloadUrl,
                request.FileName));
        }
        catch (Exception exception)
        {
            await endpoint.Send(new PdfGenerationFailed(
                request.JobId,
                request.ProjectId,
                request.UserId,
                exception.Message,
                DateTime.UtcNow));

            throw;
        }

    }
}