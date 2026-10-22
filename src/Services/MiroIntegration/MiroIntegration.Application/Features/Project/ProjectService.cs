using MiroIntegration.Application.Abstractions;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Application.Features.ProjectService
{
    public class ProjectService(IProjectRepository projectRepository) : IProjectService
    {
        public async Task<Project?> GetProjectAsync(Guid id, CancellationToken cancellationToken)
        {
            var prj =  await projectRepository.FindByIdAsync(id, cancellationToken);

            if (prj is null)
            {
                return null;
            }
            return prj;
        }
        }
    }
