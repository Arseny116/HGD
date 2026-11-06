using MiroIntegration.Application.Abstractions;
using CSharpFunctionalExtensions;
using MiroIntegration.Domain.Models;
using PdfEntity = MiroIntegration.Domain.Models.Pdf;

namespace MiroIntegration.Application.Features.Pdf
{
    public sealed class PdfServiceResult(
        IPdfRepository pdfRepository,
        IProjectRepository projectRepository) : IPdfService
    {
        public async Task<Result<PdfEntity>> CreateAsync(
            Guid userId,
            Guid projectId,
            string fileName,
            CancellationToken cancellationToken)
        {
            var project = await projectRepository.FindByIdAsync(
                projectId,
                userId,
                cancellationToken);

            if (project is null)
                return Result.Failure<PdfEntity>("Project not found.");

            var jobId = Guid.NewGuid();
            var pdfResult = PdfEntity.Create(jobId, projectId, fileName, $"{jobId}.pdf");

            if (pdfResult.IsFailure)
                return pdfResult;

            var pdf = pdfResult.Value;
            pdf.ChangeStatus(PdfEntity.State.Processing);
            await pdfRepository.AddAsync(pdf, cancellationToken);

            return Result.Success(pdf);
        }

        public Task<PdfEntity?> GetByIdAsync(
            Guid id,
            Guid userId,
            CancellationToken cancellationToken) => pdfRepository.FindByIdAsync(
                id,
                userId,
                cancellationToken);

        public async Task<IReadOnlyList<PdfEntity>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken) => await pdfRepository.ListByUserIdAsync(
                userId,
                cancellationToken);

        public async Task<IReadOnlyList<PdfEntity>> GetByProjectIdAsync(
            Guid projectId,
            Guid userId,
            CancellationToken cancellationToken) => await pdfRepository.ListByProjectIdAsync(
                projectId,
                userId,
                cancellationToken);

        public Task<Result> CompleteAsync(
            Guid jobId,
            Guid userId,
            string objectKey,
            string fileUrl,
            CancellationToken cancellationToken) => UpdateStatusAsync(
                jobId,
                userId,
                pdf => pdf.Complete(objectKey, fileUrl),
                cancellationToken);

        public Task<Result> FailAsync(
            Guid jobId,
            Guid userId,
            CancellationToken cancellationToken) => UpdateStatusAsync(
                jobId,
                userId,
                pdf =>
                {
                    pdf.ChangeStatus(PdfEntity.State.Failed);
                    return Result.Success();
                },
                cancellationToken);

        private async Task<Result> UpdateStatusAsync(
            Guid jobId,
            Guid userId,
            Func<PdfEntity, Result> update,
            CancellationToken cancellationToken)
        {
            var pdf = await pdfRepository.FindByJobIdAsync(
                jobId,
                userId,
                cancellationToken);

            if (pdf is null)
                return Result.Failure("PDF not found.");

            var result = update(pdf);
            if (result.IsFailure)
                return result;

            await pdfRepository.UpdateAsync(pdf, cancellationToken);
            return Result.Success();
        }
    }
}
