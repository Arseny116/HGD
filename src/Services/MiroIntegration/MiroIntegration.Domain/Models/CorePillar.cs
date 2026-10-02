using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models;

public sealed class CorePillar
{

    private CorePillar(string name, List<string> pillars)
    {
        Id = Guid.NewGuid();
        Name = name;
        Pillars = pillars.ToList();
        CreatedAt = DateTimeOffset.UtcNow;
    }
    public   Guid Id { get; private set; }
    public  string Name { get; private set; } = string.Empty;
    public  List<string> Pillars { get; private set; } = [];
  
    public  DateTimeOffset CreatedAt { get; private set; }
    public  DateTimeOffset UpdatedAt { get; private set;}

    public static Result<CorePillar> Create(string name, List<string> pillars)
    {

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<CorePillar>("Name cannot be empty.");
        }

        if (pillars is null)
        {
            return Result.Failure<CorePillar>("Pillars cannot be null.");
        }


        CorePillar corePillar = new CorePillar(name, pillars);

        return Result.Success(corePillar);

    }


}