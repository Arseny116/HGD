namespace Shared.Contracts
{
    public class Contracts
    {
        public record PdfGenerationStartRequest(
            Guid ProjectId,
            string FileName);

        public record PdfGenerationRequest(
            Guid UserId,
            Guid JobId,
             Guid ProjectId,
             string FileName,
             ProjectPdfData Content
         );
        public record PdfGenerationCompleted(
             Guid JobId,
             Guid ProjectId,
             Guid UserId,
             string ObjectKey,
             string FileUrl,
             string FileName
            );

        public record PdfGenerationFailed(
            Guid JobId,
            Guid ProjectId,
            Guid UserId,
            string Reason,
            DateTime FailedAt
            );
    }
}
