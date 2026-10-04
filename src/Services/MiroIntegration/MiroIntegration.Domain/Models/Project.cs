using CSharpFunctionalExtensions;
using System.Net;

namespace MiroIntegration.Domain.Models
{
    public class Project
    {

        private Project(string name, List<string> pillars)
        {
            Id = Guid.NewGuid();
            Name = name;
            Pillars = pillars.ToList();
            CreatedAt = DateTimeOffset.UtcNow;
        }
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public List<string> Pillars { get; private set; } = [];

        public List<Pdf> PdfScope { get; private set; } = new List<Pdf>();

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        public static Result<Project> Create(string name, List<string> pillars)
        {

            if (string.IsNullOrWhiteSpace(name))
            {
                return Result.Failure<Project>("Name cannot be empty.");
            }

            if (pillars is null)
            {
                return Result.Failure<Project>("Pillars cannot be null.");
            }


            Project Project = new Project(name, pillars);

            return Result.Success(Project);

        }



        public Result<Project> AddPdf(Pdf pdf)
        {
            if (PdfScope.Count >= 5)
                return Result.Failure<Project>("Cannot add more than 5 PDFs.");
            PdfScope.Add(pdf);
            UpdatedAt = DateTimeOffset.UtcNow;
            return Result.Success(this);
        }

    }
}
