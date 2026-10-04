using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models
{
    public  class Pdf
    {
        public Guid Id { get; private set; }
        public Guid JobId { get; private set; }

        public string FileName { get; private set; } = string.Empty;

        public string FileUrl  { get; private set; } = string.Empty;

        public State Status { get; private set; } = State.Pending;

        public Guid ProjectId { get; private set; }

        public Project Project { get; private set; } = null!;

      

        

        private Pdf(Guid jobId, Guid projectId, string fileName, string fileUrl)
        {
            Id = Guid.NewGuid();
            JobId = jobId;
            ProjectId = projectId;
            FileName = fileName;
            FileUrl = fileUrl;
        }
        
        public static Result<Pdf> Create(Guid jobId, Guid projectId, string fileName, string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return Result.Failure<Pdf>("File name cannot be empty.");
            if (string.IsNullOrWhiteSpace(fileUrl))
                return Result.Failure<Pdf>("File URL cannot be empty.");
            return Result.Success(new Pdf(jobId, projectId, fileName, fileUrl));
        }

        public void ChangeStatus(State status)
        {
            Status = status;
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
