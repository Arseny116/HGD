using MiroIntegration.Domain.Models;

namespace Shared.Contracts
{
    public class Contracts
    {
        public record PdfGenerationRequest(
            Guid UserId,
            Guid JobId,
            string FileName,
            Project Content
         );
        public record PdfGenerationCompleted(
             Guid JobId,
             Guid UserId,
             string FileUrl,
             string FileName,
             DateTime CompletedAt
            );

        public record PdfGenerationFailed(
            Guid JobId,
            Guid UserId,
            string Reason,
            DateTime FailedAt
            );
    }
}
