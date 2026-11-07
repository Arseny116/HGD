using AutoFixture;
using FluentAssertions;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Domain.Tests;

public sealed class ProjectTests : TestFixture
{
    [Fact]
    public void Create_ShouldFail_WhenUserIdIsEmpty()
    {
        var result = Project.Create(
            Guid.Empty,
            Fixture.Create<string>(),
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("UserId cannot be empty.");
    }

    [Fact]
    public void Create_ShouldFail_WhenNameIsEmpty()
    {
        var result = Project.Create(
            Fixture.Create<Guid>(),
            string.Empty,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!,
            null!);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Name cannot be empty.");
    }
}
