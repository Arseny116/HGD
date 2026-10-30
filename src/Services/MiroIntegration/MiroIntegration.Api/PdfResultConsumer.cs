using MassTransit;
using MiroIntegration.Services;
using static Shared.Contracts.Contracts;

namespace MiroIntegration.API
{
    public class PdfResultConsumer(PdfNotifier pdfNotifier) : IConsumer<PdfGenerationCompleted>
    {
        public async Task Consume(ConsumeContext<PdfGenerationCompleted> context)
        {
            var message = context.Message;
            await pdfNotifier.NotifyPdfReadyAsync(
                message.UserId,
                message.JobId,
                message.FileUrl
                );
        }
    }
}
