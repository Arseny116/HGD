using AutoMapper;
using MiroIntegration.Domain.Models;
using Shared.Contracts;

namespace MiroIntegration.Application.Features.ProjectService;

public sealed class ProjectPdfMappingProfile : Profile
{
    public ProjectPdfMappingProfile()
    {
        CreateMap<Project, ProjectPdfData>();
        CreateMap<MetaBlock, MetaBlockDto>();
        CreateMap<TechnicalSpecsBlock, TechnicalSpecsBlockDto>();
        CreateMap<GameplayBlock, GameplayBlockDto>();
        CreateMap<GameplayOutlineItem, GameplayOutlineItemDto>();
        CreateMap<DesignDocumentBlock, DesignDocumentBlockDto>();
        CreateMap<DesignDefinition, DesignDefinitionDto>();
        CreateMap<FlowchartBlock, FlowchartBlockDto>();
        CreateMap<FlowchartNode, FlowchartNodeDto>()
            .ForMember(destination => destination.Type,
                options => options.MapFrom(source => source.Type.ToString()));
        CreateMap<FlowchartEdge, FlowchartEdgeDto>();
        CreateMap<PlayerBlock, PlayerBlockDto>();
        CreateMap<PlayerDefinition, PlayerDefinitionDto>();
        CreateMap<PlayerProperty, PlayerPropertyDto>();
        CreateMap<PlayerReward, PlayerRewardDto>()
            .ForMember(destination => destination.Type,
                options => options.MapFrom(source => source.Type.ToString()));
        CreateMap<UiBlock, UiBlockDto>();
        CreateMap<UiControl, UiControlDto>()
            .ForMember(destination => destination.Type,
                options => options.MapFrom(source => source.Type.ToString()));
    }
}
