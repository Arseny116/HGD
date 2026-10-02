using AutoMapper;
using FluentValidation;
using MediatR;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Domain.Models;
using System.Text.Json.Serialization;

namespace MiroIntegration.Application.Features.Auth;

public sealed record UserResponse(Guid Id, string Name, string Email);
public sealed record AuthResponse(string Token, UserResponse User);
public sealed record RegisterCommand(
    string Name,
    string Email,
    string Password,
    [property: JsonPropertyName("password_confirmation")] string PasswordConfirmation) : IRequest<AuthResponse>;
public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;
public sealed record GetCurrentUserQuery(Guid UserId) : IRequest<UserResponse>;

public sealed class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.PasswordConfirmation).Equal(x => x.Password).WithMessage("Passwords do not match.");
    }
}

public sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public sealed class AuthMappingProfile : Profile
{
    public AuthMappingProfile() => CreateMap<User, UserResponse>();
}

public sealed class RegisterHandler(IUserRepository users, IPasswordHasher hasher, ITokenService tokens, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<RegisterCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await users.FindByEmailAsync(email, cancellationToken) is not null)
            throw new ValidationException("Email is already registered.");

        var user = new User(request.Name.Trim(), email, hasher.Hash(request.Password));
        await users.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new AuthResponse(tokens.CreateToken(user), mapper.Map<UserResponse>(user));
    }
}

public sealed class LoginHandler(IUserRepository users, IPasswordHasher hasher, ITokenService tokens, IMapper mapper) : IRequestHandler<LoginCommand, AuthResponse>
{
    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await users.FindByEmailAsync(request.Email.Trim().ToLowerInvariant(), cancellationToken);
        if (user is null || !hasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return new AuthResponse(tokens.CreateToken(user), mapper.Map<UserResponse>(user));
    }
}

public sealed class GetCurrentUserHandler(IUserRepository users, IMapper mapper) : IRequestHandler<GetCurrentUserQuery, UserResponse>
{
    public async Task<UserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await users.FindByIdAsync(request.UserId, cancellationToken) ?? throw new KeyNotFoundException("User not found.");
        return mapper.Map<UserResponse>(user);
    }
}