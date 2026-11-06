using MassTransit;
using MiroIntegration.Services;
using MiroIntegration.Application.Abstractions;
using static Shared.Contracts.Contracts;

namespace MiroIntegration.API
{
    public class PdfResultConsumer(PdfNotifier pdfNotifier, IPdfService service) : IConsumer<PdfGenerationCompleted>
    {
        public async Task Consume(ConsumeContext<PdfGenerationCompleted> context)
        {
            var message = context.Message;

            await service.CompleteAsync(
                message.JobId,
                message.UserId,
                message.ObjectKey,
                message.FileUrl,
                context.CancellationToken);

            await pdfNotifier.NotifyPdfReadyAsync(
                message.UserId,
                message.JobId,
                message.FileUrl
                );
        }
    }

    public class PdfFailureConsumer(IPdfService service, PdfNotifier pdfNotifier)
        : IConsumer<PdfGenerationFailed>
    {
        public async Task Consume(ConsumeContext<PdfGenerationFailed> context)
        {
            var message = context.Message;

            await service.FailAsync(
                message.JobId,
                message.UserId,
                context.CancellationToken);

            await pdfNotifier.NotifyPdfFailedAsync(
                message.UserId,
                message.JobId,
                message.Reason);
        }
    }
}
