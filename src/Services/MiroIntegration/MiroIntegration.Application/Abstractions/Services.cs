using CSharpFunctionalExtensions;
using MiroIntegration.Application.Projects.DTOs;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Application.Abstractions;


public interface IProjectService 
{
    public  Task<Project?> GetProjectAsync(Guid id, CancellationToken cancellationToken);
    public Task<Result<Guid>> CreateProjectAsync(Guid userId,CreateProjectRequest request,CancellationToken ct);
};


