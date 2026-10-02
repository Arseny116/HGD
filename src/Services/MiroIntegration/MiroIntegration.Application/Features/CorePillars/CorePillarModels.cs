using AutoMapper;
using FluentValidation;
using MediatR;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Domain.Models;
using System.Text.Json;

namespace MiroIntegration.Application.Features.CorePillars;

public sealed record CreateCorePillarCommand(string Name, object? CorePillars) : IRequest<CorePillarResponse>;
public sealed record CorePillarResponse(Guid Id, string Name, IReadOnlyCollection<string> Pillars, string? PdfPath, DateTimeOffset CreatedAt);

public sealed class CreateCorePillarValidator : AbstractValidator<CreateCorePillarCommand>
{
    public CreateCorePillarValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CorePillars).NotNull();
    }
}

public sealed class CorePillarMappingProfile : Profile
{
    public CorePillarMappingProfile() => CreateMap<CorePillar, CorePillarResponse>();
}

public sealed class CreateCorePillarHandler(ICorePillarRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCorePillarCommand, CorePillarResponse>
{
    public async Task<CorePillarResponse> Handle(CreateCorePillarCommand request, CancellationToken cancellationToken)
    {
        var values = request.CorePillars switch
        {
            string text => text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            JsonElement json when json.ValueKind == JsonValueKind.String => json.GetString()?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [],
            JsonElement json when json.ValueKind == JsonValueKind.Array => json.EnumerateArray().Select(x => x.GetString()?.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Cast<string>(),
            IEnumerable<object> items => items.Select(x => x?.ToString()?.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Cast<string>(),
            _ => []
        };
        var entity = new CorePillar(request.Name.Trim(), values.ToList());
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return mapper.Map<CorePillarResponse>(entity);
    }
}