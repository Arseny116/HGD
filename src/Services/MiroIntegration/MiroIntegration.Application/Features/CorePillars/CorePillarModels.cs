using AutoMapper;
using FluentValidation;
using MediatR;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Domain.Models;
using System.Text.Json;

namespace MiroIntegration.Application.Features.Projects;

public sealed record CreateProjectCommand(string Name, object? Projects) : IRequest<ProjectResponse>;
public sealed record ProjectResponse(Guid Id, string Name, IReadOnlyCollection<string> Pillars, string? PdfPath, DateTimeOffset CreatedAt);

public sealed class CreateProjectValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Projects).NotNull();
    }
}

public sealed class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile() => CreateMap<Project, ProjectResponse>();
}

public sealed class CreateProjectHandler(IProjectRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateProjectCommand, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var values = request.Projects switch
        {
            string text => text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            JsonElement json when json.ValueKind == JsonValueKind.String => json.GetString()?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [],
            JsonElement json when json.ValueKind == JsonValueKind.Array => json.EnumerateArray().Select(x => x.GetString()?.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Cast<string>(),
            IEnumerable<object> items => items.Select(x => x?.ToString()?.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Cast<string>(),
            _ => []
        };
        var entity = new Project(request.Name.Trim(), values.ToList());
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<ProjectResponse>(entity);
    }
}