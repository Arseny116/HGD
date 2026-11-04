using MassTransit;
using PdfWorker.Services.Storage;
using static Shared.Contracts.Contracts;

namespace PdfWorker.Services;

public sealed class PdfGenerationConsumer : IConsumer<PdfGenerationRequest>
{
    private readonly PdfGenerator _pdfGenerator;
    private readonly S3FileStorage _fileStorage;

    public PdfGenerationConsumer(PdfGenerator pdfGenerator,S3FileStorage fileStorage)
    {
        _pdfGenerator = pdfGenerator;
        _fileStorage = fileStorage;
    }

    public async Task Consume(ConsumeContext<PdfGenerationRequest> context)
    {
        var request = context.Message;

        var pdfBytes = _pdfGenerator.Generate(request);

        var objectKey = $"{request.JobId}.pdf";

        await _fileStorage.UploadAsync(
            pdfBytes,
            objectKey,
            context.CancellationToken);
    }
}