using AutoFixture;
using FluentAssertions;
using MiroIntegration.Domain.Models;

namespace MiroIntegration.Domain.Tests;

public sealed class CoreModelTests : TestFixture
{
    [Fact]
    public void MetaBlock_Create_ShouldSucceed_WithValidData()
    {
        var result = MetaBlock.Create("Test game", "Action", "Combat", 1);

        result.IsSuccess.Should().BeTrue();
        result.Value.GameName.Should().Be("Test game");
        result.Value.PlayerCount.Should().Be(1);
    }

    [Fact]
    public void MetaBlock_Create_ShouldFail_WhenPlayerCountIsZero()
    {
        var result = MetaBlock.Create("Test game", "Action", "Combat", 0);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("PlayerCount must be at least 1.");
    }

    [Fact]
    public void TechnicalSpecsBlock_Create_ShouldFail_WhenFormIsInvalid()
    {
        var result = TechnicalSpecsBlock.Create(
            "Invalid",
            "Third person",
            ["PC"],
            ["C#"]);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("TechnicalForm must be 2D or 3D.");
    }

    [Fact]
    public void UiBlock_Create_ShouldFail_WhenNoLayoutDataIsProvided()
    {
        var result = UiBlock.Create(null, null);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Either LayoutImageUrl or LayoutDescription must be provided.");
    }

    [Fact]
    public void Pdf_Create_ShouldSucceed_WithValidData()
    {
        var result = Pdf.Create(
            Fixture.Create<Guid>(),
            Fixture.Create<Guid>(),
            "document.pdf",
            "documents/document.pdf");

        result.IsSuccess.Should().BeTrue();
        result.Value.Status.Should().Be(Pdf.State.Pending);
    }

    [Fact]
    public void Pdf_Complete_ShouldSetCompletedStatus()
    {
        var pdf = Pdf.Create(
            Fixture.Create<Guid>(),
            Fixture.Create<Guid>(),
            "document.pdf",
            "documents/document.pdf").Value;

        var result = pdf.Complete(
            "documents/completed.pdf",
            "https://storage.test/completed.pdf");

        result.IsSuccess.Should().BeTrue();
        pdf.Status.Should().Be(Pdf.State.Completed);
        pdf.ObjectKey.Should().Be("documents/completed.pdf");
        pdf.FileUrl.Should().Be("https://storage.test/completed.pdf");
    }

    [Fact]
    public void User_Create_ShouldFail_WhenEmailIsEmpty()
    {
        var result = User.Create("Test user", string.Empty, "hash");

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Email cannot be empty.");
    }
}
