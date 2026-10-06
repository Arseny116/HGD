using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models
{
    public class Project
    {
        private Project(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public User User { get; private set; } = null!;
        public string Name { get; private set; } = string.Empty;

        public MetaBlock? Meta { get; private set; }
        public TechnicalSpecsBlock? TechnicalSpecs { get; private set; }
        public GameplayBlock? Gameplay { get; private set; }
        public DesignDocumentBlock? DesignDocument { get; private set; }
        public FlowchartBlock? Flowchart { get; private set; }
        public PlayerBlock? Player { get; private set; }
        public UiBlock? Ui { get; private set; }

        public List<Pdf> PdfScope { get; private set; } = new List<Pdf>();

        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        public static Result<Project> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Project>("Name cannot be empty.");

            return Result.Success(new Project(name));
        }

        public Result SetMeta(MetaBlock meta)
        {
            if (meta is null) return Result.Failure("Meta cannot be null.");
            Meta = meta;
            UpdatedAt = DateTimeOffset.UtcNow;
            return Result.Success();
        }

        public Result SetTechnicalSpecs(TechnicalSpecsBlock tech)
        {
            if (tech is null) return Result.Failure("TechnicalSpecs cannot be null.");
            TechnicalSpecs = tech;
            UpdatedAt = DateTimeOffset.UtcNow;
            return Result.Success();
        }

        public Result SetGameplay(GameplayBlock gameplay)
        {
            if (gameplay is null) return Result.Failure("Gameplay cannot be null.");
            Gameplay = gameplay;
            UpdatedAt = DateTimeOffset.UtcNow;
            return Result.Success();
        }

        public Result SetDesignDocument(DesignDocumentBlock design)
        {
            if (design is null) return Result.Failure("DesignDocument cannot be null.");
            DesignDocument = design;
            UpdatedAt = DateTimeOffset.UtcNow;
            return Result.Success();
        }

        public Result SetFlowchart(FlowchartBlock flowchart)
        {
            if (flowchart is null) return Result.Failure("Flowchart cannot be null.");
            Flowchart = flowchart;
            UpdatedAt = DateTimeOffset.UtcNow;
            return Result.Success();
        }

        public Result SetPlayer(PlayerBlock player)
        {
            if (player is null) return Result.Failure("Player cannot be null.");
            Player = player;
            UpdatedAt = DateTimeOffset.UtcNow;
            return Result.Success();
        }

        public Result SetUi(UiBlock ui)
        {
            if (ui is null) return Result.Failure("Ui cannot be null.");
            Ui = ui;
            UpdatedAt = DateTimeOffset.UtcNow;
            return Result.Success();
        }
    }
}