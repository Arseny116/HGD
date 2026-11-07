using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.IntegrationTests;

public sealed class UserRepositoryTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;

    public UserRepositoryTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task AddAsync_ShouldPersistUser_AndFindByEmail()
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var user = User.Create(
            "Integration user",
            $"{Guid.NewGuid():N}@test.local",
            "password-hash").Value;

        await repository.AddAsync(user, CancellationToken.None);

        var result = await repository.FindByEmailAsync(
            user.Email,
            CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(user.Id);
        result.Email.Should().Be(user.Email);
    }
}
