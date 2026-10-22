using MassTransit;
using static Shared.Contracts.Contracts;

public sealed class PdfGenerationConsumer : IConsumer<PdfGenerationRequest>
{
    public async Task Consume(
        ConsumeContext<PdfGenerationRequest> context)
    {
        var request = context.Message;


        await Task.CompletedTask;
    }
}