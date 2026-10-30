using MiroIntegration.Domain.Models;

namespace MiroIntegration.Application.Projects.DTOs;

public sealed record CreateProjectRequest(
    string Name,
    MetaBlockDto Meta,
    TechnicalSpecsBlockDto TechnicalSpecs,
    GameplayBlockDto Gameplay,
    DesignDocumentBlockDto DesignDocument,
    FlowchartBlockDto Flowchart,
    PlayerBlockDto Player,
    UiBlockDto UI
);

// MetaBlock
public sealed record MetaBlockDto(
    string GameName,
    string Genre,
    string GameElements,
    int PlayerCount,
    string? CoverImageUrl
);

// TechnicalSpecsBlock
public sealed record TechnicalSpecsBlockDto(
    string TechnicalForm,
    string View,
    List<string> Platforms,
    List<string> Languages,
    string? ReferenceImageUrl
);

// GameplayBlock
public sealed record GameplayBlockDto(
    string Description,
    List<GameplayOutlineItemDto> Outline,
    string? ReferenceImageUrl
);

public sealed record GameplayOutlineItemDto(
    string Text,
    int Order
);

// DesignDocumentBlock
public sealed record DesignDocumentBlockDto(
    string Guidelines,
    List<DesignDefinitionDto> Definitions
);

public sealed record DesignDefinitionDto(
    string Key,
    string Value
);

// FlowchartBlock
public sealed record FlowchartBlockDto(
    List<FlowchartNodeDto> Nodes,
    List<FlowchartEdgeDto> Edges
);

public sealed record FlowchartNodeDto(
    Guid ClientId,
    string Label,
    FlowchartNodeType Type,
    double X,
    double Y,
    string? Color
);

public sealed record FlowchartEdgeDto(
    Guid FromNodeClientId,
    Guid ToNodeClientId,
    string? Label
);

// PlayerBlock
public sealed record PlayerBlockDto(
    PlayerDefinitionDto Definition,
    List<PlayerPropertyDto> Properties,
    List<PlayerRewardDto> Rewards,
    string? ReferenceImageUrl
);

public sealed record PlayerDefinitionDto(
    int Health,
    List<string> Weapons,
    List<string> Actions
);

public sealed record PlayerPropertyDto(
    string Name,
    string Description,
    string? Feedback
);

public sealed record PlayerRewardDto(
    string Name,
    string Effect,
    PlayerRewardType Type,
    string? IconUrl
);

// UiBlock
public sealed record UiBlockDto(
    List<UiControlDto> Controls,
    string? LayoutImageUrl,
    string? LayoutDescription
);

public sealed record UiControlDto(
    string Name,
    string Action,
    UiControlType Type,
    string? IconUrl
);