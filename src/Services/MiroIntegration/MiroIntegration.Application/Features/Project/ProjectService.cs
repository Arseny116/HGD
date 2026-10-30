using CSharpFunctionalExtensions;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Application.Projects.DTOs;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Application.Features.ProjectService;

public sealed class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    public async Task<Project?> GetProjectAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await projectRepository.FindByIdAsync(id, cancellationToken);
    }

    public async Task<Result<Guid>> CreateProjectAsync(
        Guid userId,
        CreateProjectRequest request,
        CancellationToken ct)
    {
        // 1. MetaBlock
        var metaResult = MetaBlock.Create(
            request.Meta.GameName,
            request.Meta.Genre,
            request.Meta.GameElements,
            request.Meta.PlayerCount,
            request.Meta.CoverImageUrl);

        if (metaResult.IsFailure)
            return Result.Failure<Guid>(metaResult.Error);

        var meta = metaResult.Value;

        // 2. TechnicalSpecsBlock
        var techResult = TechnicalSpecsBlock.Create(
            request.TechnicalSpecs.TechnicalForm,
            request.TechnicalSpecs.View,
            request.TechnicalSpecs.Platforms,
            request.TechnicalSpecs.Languages,
            request.TechnicalSpecs.ReferenceImageUrl);

        if (techResult.IsFailure)
            return Result.Failure<Guid>(techResult.Error);

        var technicalSpecs = techResult.Value;

        // 3. GameplayBlock и Outline
        var outline = new List<GameplayOutlineItem>();

        foreach (var outlineItemDto in request.Gameplay.Outline)
        {
            var outlineItemResult = GameplayOutlineItem.Create(
                outlineItemDto.Text,
                outlineItemDto.Order);

            if (outlineItemResult.IsFailure)
                return Result.Failure<Guid>(outlineItemResult.Error);

            outline.Add(outlineItemResult.Value);
        }

        var gameplayResult = GameplayBlock.Create(
            request.Gameplay.Description,
            outline,
            request.Gameplay.ReferenceImageUrl);

        if (gameplayResult.IsFailure)
            return Result.Failure<Guid>(gameplayResult.Error);

        var gameplay = gameplayResult.Value;

        // 4. DesignDocumentBlock и Definitions
        var designDefinitions = new List<DesignDefinition>();

        foreach (var designDefinitionDto in request.DesignDocument.Definitions)
        {
            var designDefinitionResult = DesignDefinition.Create(
                designDefinitionDto.Key,
                designDefinitionDto.Value);

            if (designDefinitionResult.IsFailure)
                return Result.Failure<Guid>(designDefinitionResult.Error);

            designDefinitions.Add(designDefinitionResult.Value);
        }

        var designDocumentResult = DesignDocumentBlock.Create(
            request.DesignDocument.Guidelines,
            designDefinitions);

        if (designDocumentResult.IsFailure)
            return Result.Failure<Guid>(designDocumentResult.Error);

        var designDocument = designDocumentResult.Value;

        // 5. FlowchartBlock: сначала создаём узлы
        var nodes = new List<FlowchartNode>();
        var nodeIds = new Dictionary<Guid, Guid>();

        foreach (var nodeDto in request.Flowchart.Nodes)
        {
            if (nodeIds.ContainsKey(nodeDto.ClientId))
            {
                return Result.Failure<Guid>(
                    $"Duplicate flowchart node ClientId: {nodeDto.ClientId}");
            }

            var nodeResult = FlowchartNode.Create(
                nodeDto.Label,
                nodeDto.Type,
                nodeDto.X,
                nodeDto.Y,
                nodeDto.Color);

            if (nodeResult.IsFailure)
                return Result.Failure<Guid>(nodeResult.Error);

            var node = nodeResult.Value;

            nodes.Add(node);
            nodeIds.Add(nodeDto.ClientId, node.Id);
        }

        // Создаём рёбра, используя настоящие ID узлов
        var edges = new List<FlowchartEdge>();

        foreach (var edgeDto in request.Flowchart.Edges)
        {
            if (!nodeIds.TryGetValue(
                    edgeDto.FromNodeClientId, out var fromNodeId))
            {
                return Result.Failure<Guid>(
                    $"Source flowchart node not found: {edgeDto.FromNodeClientId}");
            }

            if (!nodeIds.TryGetValue(
                    edgeDto.ToNodeClientId, out var toNodeId))
            {
                return Result.Failure<Guid>(
                    $"Target flowchart node not found: {edgeDto.ToNodeClientId}");
            }

            var edgeResult = FlowchartEdge.Create(
                fromNodeId,
                toNodeId,
                edgeDto.Label);

            if (edgeResult.IsFailure)
                return Result.Failure<Guid>(edgeResult.Error);

            edges.Add(edgeResult.Value);
        }

        var flowchartResult = FlowchartBlock.Create(nodes, edges);

        if (flowchartResult.IsFailure)
            return Result.Failure<Guid>(flowchartResult.Error);

        var flowchart = flowchartResult.Value;

        // 6. PlayerBlock и вложенные сущности
        var playerDefinitionDto = request.Player.Definition;

        var playerDefinitionResult = PlayerDefinition.Create(
            playerDefinitionDto.Health,
            playerDefinitionDto.Weapons,
            playerDefinitionDto.Actions);

        if (playerDefinitionResult.IsFailure)
            return Result.Failure<Guid>(playerDefinitionResult.Error);

        var playerDefinition = playerDefinitionResult.Value;

        var playerResult = PlayerBlock.Create(
            playerDefinition,
            request.Player.ReferenceImageUrl);

        if (playerResult.IsFailure)
            return Result.Failure<Guid>(playerResult.Error);

        var player = playerResult.Value;

        foreach (var propertyDto in request.Player.Properties)
        {
            var propertyResult = PlayerProperty.Create(
                propertyDto.Name,
                propertyDto.Description,
                propertyDto.Feedback);

            if (propertyResult.IsFailure)
                return Result.Failure<Guid>(propertyResult.Error);

            var addPropertyResult = player.AddProperty(propertyResult.Value);

            if (addPropertyResult.IsFailure)
                return Result.Failure<Guid>(addPropertyResult.Error);
        }

        foreach (var rewardDto in request.Player.Rewards)
        {
            var rewardResult = PlayerReward.Create(
                rewardDto.Name,
                rewardDto.Effect,
                rewardDto.Type,
                rewardDto.IconUrl);

            if (rewardResult.IsFailure)
                return Result.Failure<Guid>(rewardResult.Error);

            var addRewardResult = player.AddReward(rewardResult.Value);

            if (addRewardResult.IsFailure)
                return Result.Failure<Guid>(addRewardResult.Error);
        }

        // 7. UiBlock и Controls
        var uiResult = UiBlock.Create(
            request.UI.LayoutImageUrl,
            request.UI.LayoutDescription);

        if (uiResult.IsFailure)
            return Result.Failure<Guid>(uiResult.Error);

        var ui = uiResult.Value;

        foreach (var controlDto in request.UI.Controls)
        {
            var controlResult = UiControl.Create(
                controlDto.Name,
                controlDto.Action,
                controlDto.Type,
                controlDto.IconUrl);

            if (controlResult.IsFailure)
                return Result.Failure<Guid>(controlResult.Error);

            var addControlResult = ui.AddControl(controlResult.Value);

            if (addControlResult.IsFailure)
                return Result.Failure<Guid>(addControlResult.Error);
        }

        // 8. Собираем Project со всеми блоками и UserId
        var projectResult = Project.Create(
            userId,
            request.Name,
            meta,
            technicalSpecs,
            gameplay,
            designDocument,
            flowchart,
            player,
            ui);

        if (projectResult.IsFailure)
            return Result.Failure<Guid>(projectResult.Error);

        var project = projectResult.Value;

        // 9. Сохраняем весь агрегат
        await projectRepository.AddAsync(project, ct);

        return Result.Success(project.Id);
    }
}