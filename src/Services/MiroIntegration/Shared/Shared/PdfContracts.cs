namespace Shared.Contracts;

public sealed record PdfDto(
    Guid Id,
    Guid JobId,
    Guid ProjectId,
    string FileName,
    string Status,
    string? FileUrl);

public sealed class ProjectPdfData
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public MetaBlockDto? Meta { get; init; }
    public TechnicalSpecsBlockDto? TechnicalSpecs { get; init; }
    public GameplayBlockDto? Gameplay { get; init; }
    public DesignDocumentBlockDto? DesignDocument { get; init; }
    public FlowchartBlockDto? Flowchart { get; init; }
    public PlayerBlockDto? Player { get; init; }
    public UiBlockDto? Ui { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}

public sealed class MetaBlockDto
{
    public string GameName { get; init; } = string.Empty;
    public string Genre { get; init; } = string.Empty;
    public string GameElements { get; init; } = string.Empty;
    public int PlayerCount { get; init; }
    public string? CoverImageUrl { get; init; }
}

public sealed class TechnicalSpecsBlockDto
{
    public string TechnicalForm { get; init; } = string.Empty;
    public string View { get; init; } = string.Empty;
    public List<string> Platforms { get; init; } = [];
    public List<string> Languages { get; init; } = [];
    public string? ReferenceImageUrl { get; init; }
}

public sealed class GameplayBlockDto
{
    public string Description { get; init; } = string.Empty;
    public List<GameplayOutlineItemDto> Outline { get; init; } = [];
    public string? ReferenceImageUrl { get; init; }
}

public sealed class GameplayOutlineItemDto
{
    public string Text { get; init; } = string.Empty;
    public int Order { get; init; }
}

public sealed class DesignDocumentBlockDto
{
    public string Guidelines { get; init; } = string.Empty;
    public List<DesignDefinitionDto> Definitions { get; init; } = [];
}

public sealed class DesignDefinitionDto
{
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}

public sealed class FlowchartBlockDto
{
    public List<FlowchartNodeDto> Nodes { get; init; } = [];
    public List<FlowchartEdgeDto> Edges { get; init; } = [];
}

public sealed class FlowchartNodeDto
{
    public Guid Id { get; init; }
    public string Label { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public double X { get; init; }
    public double Y { get; init; }
    public string? Color { get; init; }
}

public sealed class FlowchartEdgeDto
{
    public Guid FromNodeId { get; init; }
    public Guid ToNodeId { get; init; }
    public string? Label { get; init; }
}

public sealed class PlayerBlockDto
{
    public PlayerDefinitionDto? Definition { get; init; }
    public List<PlayerPropertyDto> Properties { get; init; } = [];
    public List<PlayerRewardDto> Rewards { get; init; } = [];
    public string? ReferenceImageUrl { get; init; }
}

public sealed class PlayerDefinitionDto
{
    public int Health { get; init; }
    public List<string> Weapons { get; init; } = [];
    public List<string> Actions { get; init; } = [];
}

public sealed class PlayerPropertyDto
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? Feedback { get; init; }
}

public sealed class PlayerRewardDto
{
    public string Name { get; init; } = string.Empty;
    public string Effect { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string? IconUrl { get; init; }
}

public sealed class UiBlockDto
{
    public List<UiControlDto> Controls { get; init; } = [];
    public string? LayoutImageUrl { get; init; }
    public string? LayoutDescription { get; init; }
}

public sealed class UiControlDto
{
    public string Name { get; init; } = string.Empty;
    public string Action { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string? IconUrl { get; init; }
}
