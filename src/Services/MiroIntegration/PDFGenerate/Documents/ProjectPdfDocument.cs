using MiroIntegration.Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

// Замени на namespace, в котором объявлен Project.
// using MiroIntegration.Domain.Entities;

namespace PdfWorker.Documents;

public sealed class ProjectPdfDocument(Project project) : IDocument
{
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(40);
            page.DefaultTextStyle(x => x.FontSize(10));

            page.Header().Column(header =>
            {
                header.Item()
                    .Text(project.Name)
                    .FontSize(22)
                    .Bold();

                header.Item()
                    .PaddingTop(4)
                    .Text($"Game Design Document | {project.CreatedAt:dd.MM.yyyy}");
            });

            page.Content()
                .PaddingVertical(20)
                .Column(content =>
                {
                    // 1. Основная информация
                    content.Item().Element(SectionTitle)
                        .Text("1. Game Overview");

                    if (project.Meta is not null)
                    {
                        content.Item().Element(BodyText)
                            .Text($"Game name: {project.Meta.GameName}");

                        content.Item().Element(BodyText)
                            .Text($"Genre: {project.Meta.Genre}");

                        content.Item().Element(BodyText)
                            .Text($"Player count: {project.Meta.PlayerCount}");

                        content.Item().Element(BodyText)
                            .Text($"Core game elements: {project.Meta.GameElements}");

                        AddOptionalUrl(
                            content,
                            "Cover image",
                            project.Meta.CoverImageUrl);
                    }

                    // 2. Технические характеристики
                    content.Item().PaddingTop(15)
                        .Element(SectionTitle)
                        .Text("2. Technical Specifications");

                    if (project.TechnicalSpecs is not null)
                    {
                        content.Item().Element(BodyText)
                            .Text($"Graphics: {project.TechnicalSpecs.TechnicalForm}");

                        content.Item().Element(BodyText)
                            .Text($"Camera view: {project.TechnicalSpecs.View}");

                        AddList(
                            content,
                            "Platforms",
                            project.TechnicalSpecs.Platforms);

                        AddList(
                            content,
                            "Programming languages",
                            project.TechnicalSpecs.Languages);

                        AddOptionalUrl(
                            content,
                            "Reference image",
                            project.TechnicalSpecs.ReferenceImageUrl);
                    }

                    // 3. Игровой процесс
                    content.Item().PaddingTop(15)
                        .Element(SectionTitle)
                        .Text("3. Gameplay");

                    if (project.Gameplay is not null)
                    {
                        content.Item().Element(BodyText)
                            .Text(project.Gameplay.Description);

                        AddOptionalUrl(
                            content,
                            "Gameplay reference",
                            project.Gameplay.ReferenceImageUrl);

                        content.Item().PaddingTop(8)
                            .Text("Gameplay outline")
                            .Bold();

                        foreach (var item in (project.Gameplay.Outline
                                     ?? Enumerable.Empty<GameplayOutlineItem>())
                                 .OrderBy(x => x.Order))
                        {
                            content.Item().PaddingLeft(10)
                                .PaddingTop(4)
                                .Text($"{item.Order}. {item.Text}");
                        }
                    }

                    // 4. Механики и правила
                    content.Item().PaddingTop(15)
                        .Element(SectionTitle)
                        .Text("4. Game Mechanics and Rules");

                    if (project.DesignDocument is not null)
                    {
                        content.Item().Element(BodyText)
                            .Text(project.DesignDocument.Guidelines);

                        content.Item().PaddingTop(8)
                            .Text("Definitions")
                            .Bold();

                        foreach (var definition in
                                 project.DesignDocument.Definitions
                                 ?? Enumerable.Empty<DesignDefinition>())
                        {
                            content.Item().PaddingTop(4)
                                .Text($"{definition.Key}: {definition.Value}");
                        }
                    }

                    // 5. Поток игры
                    content.Item().PaddingTop(15)
                        .Element(SectionTitle)
                        .Text("5. Game Flowchart");

                    if (project.Flowchart is not null)
                    {
                        content.Item().Text("Flowchart nodes").Bold();

                        foreach (var node in project.Flowchart.Nodes
                                     ?? Enumerable.Empty<FlowchartNode>())
                        {
                            content.Item().PaddingTop(4)
                                .Text(
                                    $"{node.Type}: {node.Label} " +
                                    $"(X: {node.X}, Y: {node.Y})");
                        }

                        content.Item().PaddingTop(8)
                            .Text("Connections")
                            .Bold();

                        foreach (var edge in project.Flowchart.Edges
                                     ?? Enumerable.Empty<FlowchartEdge>())
                        {
                            content.Item().PaddingTop(4)
                                .Text(
                                    $"{GetNodeLabel(project, edge.FromNodeId)} " +
                                    $"-> {GetNodeLabel(project, edge.ToNodeId)}" +
                                    (string.IsNullOrWhiteSpace(edge.Label)
                                        ? ""
                                        : $" ({edge.Label})"));
                        }
                    }

                    // 6. Игрок
                    content.Item().PaddingTop(15)
                        .Element(SectionTitle)
                        .Text("6. Player");

                    if (project.Player is not null)
                    {
                        if (project.Player.Definition is not null)
                        {
                            content.Item().Element(BodyText)
                                .Text($"Initial health: {project.Player.Definition.Health}");

                            AddList(
                                content,
                                "Weapons",
                                project.Player.Definition.Weapons);

                            AddList(
                                content,
                                "Actions",
                                project.Player.Definition.Actions);
                        }

                        content.Item().PaddingTop(8)
                            .Text("Player properties")
                            .Bold();

                        foreach (var property in project.Player.Properties
                                     ?? Enumerable.Empty<PlayerProperty>())
                        {
                            content.Item().PaddingTop(4)
                                .Text($"{property.Name}: {property.Description}");

                            if (!string.IsNullOrWhiteSpace(property.Feedback))
                            {
                                content.Item().PaddingLeft(10)
                                    .Text($"Feedback: {property.Feedback}");
                            }
                        }

                        content.Item().PaddingTop(8)
                            .Text("Rewards")
                            .Bold();

                        foreach (var reward in project.Player.Rewards
                                     ?? Enumerable.Empty<PlayerReward>())
                        {
                            content.Item().PaddingTop(4)
                                .Text(
                                    $"{reward.Name} ({reward.Type}): " +
                                    $"{reward.Effect}");
                        }

                        AddOptionalUrl(
                            content,
                            "Player reference",
                            project.Player.ReferenceImageUrl);
                    }

                    // 7. Пользовательский интерфейс
                    content.Item().PaddingTop(15)
                        .Element(SectionTitle)
                        .Text("7. User Interface");

                    if (project.Ui is not null)
                    {
                        if (!string.IsNullOrWhiteSpace(
                                project.Ui.LayoutDescription))
                        {
                            content.Item().Element(BodyText)
                                .Text(project.Ui.LayoutDescription);
                        }

                        AddOptionalUrl(
                            content,
                            "UI layout reference",
                            project.Ui.LayoutImageUrl);

                        content.Item().PaddingTop(8)
                            .Text("Controls")
                            .Bold();

                        foreach (var control in project.Ui.Controls
                                     ?? Enumerable.Empty<UiControl>())
                        {
                            content.Item().PaddingTop(4)
                                .Text(
                                    $"{control.Name} ({control.Type}): " +
                                    $"{control.Action}");

                            AddOptionalUrl(
                                content,
                                $"Icon: {control.Name}",
                                control.IconUrl);
                        }
                    }

                    // Дата последнего изменения
                    content.Item().PaddingTop(20)
                        .Element(SectionTitle)
                        .Text("Document Information");

                    content.Item().Element(BodyText)
                        .Text($"Project ID: {project.Id}");

                    content.Item().Element(BodyText)
                        .Text($"Last updated: {project.UpdatedAt:dd.MM.yyyy HH:mm}");
                });

            page.Footer()
                .AlignCenter()
                .Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                    text.Span(" of ");
                    text.TotalPages();
                });
        });
    }

    private static IContainer SectionTitle(IContainer container) =>
        container
            .Background(Colors.Grey.Lighten3)
            .Padding(8)
            .DefaultTextStyle(style => style.FontSize(14).Bold());

    private static IContainer BodyText(IContainer container) =>
        container
            .PaddingTop(5)
            .PaddingBottom(3);

    private static void AddList(
        ColumnDescriptor column,
        string title,
        IEnumerable<string>? values)
    {
        column.Item().PaddingTop(8).Text(title).Bold();

        var items = values?.ToList() ?? [];

        if (items.Count == 0)
        {
            column.Item().PaddingLeft(10).Text("Not specified");
            return;
        }

        foreach (var value in items)
        {
            column.Item().PaddingLeft(10)
                .PaddingTop(3)
                .Text($"• {value}");
        }
    }

    private static void AddOptionalUrl(
        ColumnDescriptor column,
        string title,
        string? url)
    {
        if (!string.IsNullOrWhiteSpace(url))
        {
            column.Item().PaddingTop(5)
                .Text($"{title}: {url}");
        }
    }

    private static string GetNodeLabel(
        Project project,
        Guid nodeId)
    {
        var node = project.Flowchart?.Nodes?
            .FirstOrDefault(x => x.Id == nodeId);

        return node?.Label ?? nodeId.ToString();
    }
}