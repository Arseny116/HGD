using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models;

public class Project
{
    private Project() { }
    private Project(
        Guid userId,
        string name,
        MetaBlock meta,
        TechnicalSpecsBlock technicalSpecs,
        GameplayBlock gameplay,
        DesignDocumentBlock designDocument,
        FlowchartBlock flowchart,
        PlayerBlock player,
        UiBlock ui)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Name = name;

        Meta = meta;
        TechnicalSpecs = technicalSpecs;
        Gameplay = gameplay;
        DesignDocument = designDocument;
        Flowchart = flowchart;
        Player = player;
        Ui = ui;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
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

    public List<Pdf> PdfScope { get; private set; } = new();

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static Result<Project> Create(
        Guid userId,
        string name,
        MetaBlock meta,
        TechnicalSpecsBlock technicalSpecs,
        GameplayBlock gameplay,
        DesignDocumentBlock designDocument,
        FlowchartBlock flowchart,
        PlayerBlock player,
        UiBlock ui)
    {
        if (userId == Guid.Empty)
            return Result.Failure<Project>("UserId cannot be empty.");

        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Project>("Name cannot be empty.");

        if (meta is null)
            return Result.Failure<Project>("Meta cannot be null.");

        if (technicalSpecs is null)
            return Result.Failure<Project>(
                "TechnicalSpecs cannot be null.");

        if (gameplay is null)
            return Result.Failure<Project>(
                "Gameplay cannot be null.");

        if (designDocument is null)
            return Result.Failure<Project>(
                "DesignDocument cannot be null.");

        if (flowchart is null)
            return Result.Failure<Project>(
                "Flowchart cannot be null.");

        if (player is null)
            return Result.Failure<Project>(
                "Player cannot be null.");

        if (ui is null)
            return Result.Failure<Project>("UI cannot be null.");

        return Result.Success(new Project(
            userId,
            name,
            meta,
            technicalSpecs,
            gameplay,
            designDocument,
            flowchart,
            player,
            ui));
    }

    public Result SetMeta(MetaBlock meta)
    {
        if (meta is null)
            return Result.Failure("Meta cannot be null.");

        Meta = meta;
        Touch();

        return Result.Success();
    }

    public Result SetTechnicalSpecs(TechnicalSpecsBlock tech)
    {
        if (tech is null)
            return Result.Failure(
                "TechnicalSpecs cannot be null.");

        TechnicalSpecs = tech;
        Touch();

        return Result.Success();
    }

    public Result SetGameplay(GameplayBlock gameplay)
    {
        if (gameplay is null)
            return Result.Failure(
                "Gameplay cannot be null.");

        Gameplay = gameplay;
        Touch();

        return Result.Success();
    }

    public Result SetDesignDocument(DesignDocumentBlock design)
    {
        if (design is null)
            return Result.Failure(
                "DesignDocument cannot be null.");

        DesignDocument = design;
        Touch();

        return Result.Success();
    }

    public Result SetFlowchart(FlowchartBlock flowchart)
    {
        if (flowchart is null)
            return Result.Failure(
                "Flowchart cannot be null.");

        Flowchart = flowchart;
        Touch();

        return Result.Success();
    }

    public Result SetPlayer(PlayerBlock player)
    {
        if (player is null)
            return Result.Failure(
                "Player cannot be null.");

        Player = player;
        Touch();

        return Result.Success();
    }

    public Result SetUi(UiBlock ui)
    {
        if (ui is null)
            return Result.Failure("UI cannot be null.");

        Ui = ui;
        Touch();

        return Result.Success();
    }

    private void Touch()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}