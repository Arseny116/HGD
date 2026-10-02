using CSharpFunctionalExtensions;

namespace MiroIntegration.Domain.Models;

public sealed class User
{
 

    private  User(string name, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static Result<User> Create(string name, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<User>("Name cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<User>("Email cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result.Failure<User>("Password hash cannot be empty.");
        }
        User user = new User(name, email, passwordHash);
        return Result.Success(user);
    }
}