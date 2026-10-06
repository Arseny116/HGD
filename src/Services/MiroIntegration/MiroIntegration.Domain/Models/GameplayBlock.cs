using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models
{
    public class GameplayBlock
    {
        private GameplayBlock(string description, List<GameplayOutlineItem> outline)
        {
            Id = Guid.NewGuid();
            Description = description;
            Outline = outline.ToList();
        }

        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }

        public string Description { get; private set; } = string.Empty;
        public List<GameplayOutlineItem> Outline { get; private set; } = new();

        public string? ReferenceImageUrl { get; private set; }

        public static Result<GameplayBlock> Create(
            string description,
            List<GameplayOutlineItem> outline,
            string? referenceImageUrl = null)
        {
            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<GameplayBlock>("Description cannot be empty.");

            if (outline is null)
                return Result.Failure<GameplayBlock>("Outline cannot be null.");

            var gameplay = new GameplayBlock(description, outline)
            {
                ReferenceImageUrl = referenceImageUrl
            };

            return Result.Success(gameplay);
        }

        public Result AddOutlineItem(GameplayOutlineItem item)
        {
            if (item is null) return Result.Failure("Item cannot be null.");
            Outline.Add(item);
            return Result.Success();
        }
    }

    public class GameplayOutlineItem
    {
        private GameplayOutlineItem(string text, int order)
        {
            Id = Guid.NewGuid();
            Text = text;
            Order = order;
        }

        public Guid Id { get; private set; }
        public Guid GameplayBlockId { get; private set; }

        public string Text { get; private set; } = string.Empty;
        public int Order { get; private set; }

        public static Result<GameplayOutlineItem> Create(string text, int order)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Result.Failure<GameplayOutlineItem>("Text cannot be empty.");

            return Result.Success(new GameplayOutlineItem(text, order));
        }
    }
}