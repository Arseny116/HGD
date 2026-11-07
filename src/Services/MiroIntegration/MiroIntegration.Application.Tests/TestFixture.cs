namespace MiroIntegration.Application.Tests;

public abstract class TestFixture
{
    protected AutoFixture.Fixture Fixture { get; } = new();
}
