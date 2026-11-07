using AutoFixture;
using FluentAssertions;
using Moq;
using MiroIntegration.Application.Abstractions;
using MiroIntegration.Application.Features.ProjectService;
using MiroIntegration.Application.Projects.DTOs;

namespace MiroIntegration.Application.Tests;

public sealed class ProjectServiceTests : TestFixture
{
    [Fact]
    public async Task GetProjectsAsync_ShouldReturnProjectsFromRepository()
    {
        var userId = Fixture.Create<Guid>();
        var expected = new List<MiroIntegration.Domain.Models.Project>();
        var repository = new Mock<IProjectRepository>();

        repository
            .Setup(x => x.ListByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var service = new ProjectService(repository.Object);

        var result = await service.GetProjectsAsync(userId, CancellationToken.None);

        result.Should().BeSameAs(expected);
        repository.Verify(
            x => x.ListByUserIdAsync(userId, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetProjectAsync_ShouldReturnNull_WhenProjectDoesNotExist()
    {
        var projectId = Fixture.Create<Guid>();
        var userId = Fixture.Create<Guid>();
        var repository = new Mock<IProjectRepository>();

        repository
            .Setup(x => x.FindByIdAsync(
                projectId,
                userId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((MiroIntegration.Domain.Models.Project?)null);

        var service = new ProjectService(repository.Object);

        var result = await service.GetProjectAsync(
            projectId,
            userId,
            CancellationToken.None);

        result.Should().BeNull();
        repository.Verify(
            x => x.FindByIdAsync(
                projectId,
                userId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateMetaAsync_ShouldFailBeforeRepositoryLookup_WhenRequestIsInvalid()
    {
        var repository = new Mock<IProjectRepository>();
        var service = new ProjectService(repository.Object);
        var request = new MetaBlockDto(
            string.Empty,
            "Action",
            "Combat",
            1,
            null);

        var result = await service.UpdateMetaAsync(
            Fixture.Create<Guid>(),
            Fixture.Create<Guid>(),
            request,
            CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("GameName cannot be empty.");
        repository.Verify(
            x => x.FindByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
