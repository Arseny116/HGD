using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models
{
    public class Pdf
    {
        private Pdf() { }

        public Guid Id { get; private set; }
        public Guid JobId { get; private set; }

        public string FileName { get; private set; } = string.Empty;

        public string FileUrl { get; private set; } = string.Empty;
        public string ObjectKey { get; private set; } = string.Empty;

        public State Status { get; private set; } = State.Pending;

        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; } = null!;

        private Pdf(Guid jobId, Guid projectId, string fileName, string objectKey)
        {
            Id = Guid.NewGuid();
            JobId = jobId;
            ProjectId = projectId;
            FileName = fileName;
            ObjectKey = objectKey;
        }

        public static Result<Pdf> Create(
            Guid jobId,
            Guid projectId,
            string fileName,
            string objectKey)
        {
            if (jobId == Guid.Empty)
                return Result.Failure<Pdf>("JobId cannot be empty.");

            if (projectId == Guid.Empty)
                return Result.Failure<Pdf>("ProjectId cannot be empty.");

            if (string.IsNullOrWhiteSpace(fileName))
                return Result.Failure<Pdf>("File name cannot be empty.");

            if (string.IsNullOrWhiteSpace(objectKey))
                return Result.Failure<Pdf>("Object key cannot be empty.");

            return Result.Success(new Pdf(jobId, projectId, fileName, objectKey));
        }

        public void ChangeStatus(State status)
        {
            Status = status;
        }

        public Result Complete(string objectKey, string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(objectKey))
                return Result.Failure("Object key cannot be empty.");

            if (string.IsNullOrWhiteSpace(fileUrl))
                return Result.Failure("File URL cannot be empty.");

            ObjectKey = objectKey;
            FileUrl = fileUrl;
            Status = State.Completed;
            return Result.Success();
        }

        public enum State
        {
            Pending,
            Processing,
            Completed,
            Failed
        }
    }
}
