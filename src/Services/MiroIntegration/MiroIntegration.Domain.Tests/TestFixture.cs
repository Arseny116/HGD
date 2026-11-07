namespace MiroIntegration.Domain.Tests;

public abstract class TestFixture
{
    protected AutoFixture.Fixture Fixture { get; } = new();
}
