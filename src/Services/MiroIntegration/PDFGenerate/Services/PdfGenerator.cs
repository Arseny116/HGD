using PdfWorker.Documents;
using QuestPDF.Fluent;
using static Shared.Contracts.Contracts;


namespace PdfWorker.Services;

public sealed class PdfGenerator
{
    public byte[] Generate(PdfGenerationRequest data)
    {
        var document = new ProjectPdfDocument(data.Content);

        return document.GeneratePdf();
    }
}