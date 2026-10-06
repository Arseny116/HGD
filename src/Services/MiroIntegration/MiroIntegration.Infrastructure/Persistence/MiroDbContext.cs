using Microsoft.EntityFrameworkCore;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Infrastructure.Persistence;

public sealed class MiroDbContext(DbContextOptions<MiroDbContext> options): DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(); //строка
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<MetaBlock> MetaBlocks => Set<MetaBlock>();
    public DbSet<TechnicalSpecsBlock> TechnicalSpecsBlocks => Set<TechnicalSpecsBlock>();
    public DbSet<GameplayBlock> GameplayBlocks => Set<GameplayBlock>();
    public DbSet<GameplayOutlineItem> GameplayOutlineItems => Set<GameplayOutlineItem>();
    public DbSet<DesignDocumentBlock> DesignDocumentBlocks => Set<DesignDocumentBlock>();
    public DbSet<DesignDefinition> DesignDefinitions => Set<DesignDefinition>();
    public DbSet<FlowchartBlock> FlowchartBlocks => Set<FlowchartBlock>();
    public DbSet<FlowchartNode> FlowchartNodes => Set<FlowchartNode>();
    public DbSet<FlowchartEdge> FlowchartEdges => Set<FlowchartEdge>();
    public DbSet<PlayerBlock> PlayerBlocks => Set<PlayerBlock>();
    public DbSet<PlayerDefinition> PlayerDefinitions => Set<PlayerDefinition>();
    public DbSet<PlayerProperty> PlayerProperties => Set<PlayerProperty>();
    public DbSet<PlayerReward> PlayerRewards => Set<PlayerReward>();
    public DbSet<UiBlock> UiBlocks => Set<UiBlock>();
    public DbSet<UiControl> UiControls => Set<UiControl>();

    public DbSet<Pdf> Pdfs => Set<Pdf>();

    
}