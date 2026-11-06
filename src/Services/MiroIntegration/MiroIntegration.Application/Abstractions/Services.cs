using CSharpFunctionalExtensions;
using MiroIntegration.Application.Projects.DTOs;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Application.Abstractions;


public interface IProjectService 
{
    public Task<Project?> GetProjectAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    public Task<IReadOnlyList<Project>> GetProjectsAsync(Guid userId, CancellationToken cancellationToken);
    public Task<Result<Guid>> CreateProjectAsync(Guid userId,CreateProjectRequest request,CancellationToken ct);
    public Task<Result> UpdateMetaAsync(Guid projectId, Guid userId, MetaBlockDto request, CancellationToken cancellationToken);
    public Task<Result> UpdateTechnicalSpecsAsync(Guid projectId, Guid userId, TechnicalSpecsBlockDto request, CancellationToken cancellationToken);
    public Task<Result> UpdateGameplayAsync(Guid projectId, Guid userId, GameplayBlockDto request, CancellationToken cancellationToken);
    public Task<Result> UpdateDesignDocumentAsync(Guid projectId, Guid userId, DesignDocumentBlockDto request, CancellationToken cancellationToken);
    public Task<Result> UpdateFlowchartAsync(Guid projectId, Guid userId, FlowchartBlockDto request, CancellationToken cancellationToken);
    public Task<Result> UpdatePlayerAsync(Guid projectId, Guid userId, PlayerBlockDto request, CancellationToken cancellationToken);
    public Task<Result> UpdateUiAsync(Guid projectId, Guid userId, UiBlockDto request, CancellationToken cancellationToken);
};


public interface IPdfService
{
    Task<Result<Pdf>> CreateAsync(
        Guid userId,
        Guid projectId,
        string fileName,
        CancellationToken cancellationToken);

    Task<Pdf?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Pdf>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Pdf>> GetByProjectIdAsync(Guid projectId, Guid userId, CancellationToken cancellationToken);
    Task<Result> CompleteAsync(Guid jobId, Guid userId, string objectKey, string fileUrl, CancellationToken cancellationToken);
    Task<Result> FailAsync(Guid jobId, Guid userId, CancellationToken cancellationToken);
}


