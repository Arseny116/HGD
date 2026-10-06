using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models
{
    public class TechnicalSpecsBlock
    {
        private TechnicalSpecsBlock(string technicalForm, string view, List<string> platforms, List<string> languages)
        {
            Id = Guid.NewGuid();
            TechnicalForm = technicalForm;
            View = view;
            Platforms = platforms.ToList();
            Languages = languages.ToList();
        }

        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }

        public string TechnicalForm { get; private set; } = "2D";
        public string View { get; private set; } = string.Empty;
        public List<string> Platforms { get; private set; } = new();
        public List<string> Languages { get; private set; } = new();

        public string? ReferenceImageUrl { get; private set; }

        public static Result<TechnicalSpecsBlock> Create(
            string technicalForm,
            string view,
            List<string> platforms,
            List<string> languages,
            string? referenceImageUrl = null)
        {
            if (technicalForm is not ("2D" or "3D"))
                return Result.Failure<TechnicalSpecsBlock>("TechnicalForm must be 2D or 3D.");

            if (platforms is null || platforms.Count == 0)
                return Result.Failure<TechnicalSpecsBlock>("At least one platform is required.");

            if (languages is null || languages.Count == 0)
                return Result.Failure<TechnicalSpecsBlock>("At least one language is required.");

            var tech = new TechnicalSpecsBlock(technicalForm, view, platforms, languages)
            {
                ReferenceImageUrl = referenceImageUrl
            };

            return Result.Success(tech);
        }

        public Result Update(
            string technicalForm,
            string view,
            List<string> platforms,
            List<string> languages,
            string? referenceImageUrl)
        {
            if (technicalForm is not ("2D" or "3D"))
                return Result.Failure("TechnicalForm must be 2D or 3D.");

            if (platforms is null || platforms.Count == 0)
                return Result.Failure("At least one platform is required.");

            TechnicalForm = technicalForm;
            View = view;
            Platforms = platforms.ToList();
            Languages = languages.ToList();
            ReferenceImageUrl = referenceImageUrl;

            return Result.Success();
        }
    }
}