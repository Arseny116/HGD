using MiroIntegration.Domain.Models;

namespace MiroIntegration.Application.Abstractions;


public interface IProjectService 
{
    public  Task<Project?> GetProjectAsync(Guid id, CancellationToken cancellationToken);
};


