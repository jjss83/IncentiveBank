# Unit Testing Best Practices (General)

## Goals
- Confidence: Catch regressions fast.
- Design: Encourage seams and small, pure units.
- Speed: Run locally in seconds.

## Principles
- Arrange-Act-Assert (AAA). Keep tests small and focused.
- Determinism: No time, randomness, or network without control seams.
- Isolation: Stub/mocks for collaborators; avoid global state.
- Data builders: Create minimal valid objects via builders.
- One reason to fail: Avoid multiple asserts on unrelated concerns.
- Name by behavior: `TypeName_Scenario_ExpectedResult`.

## C# with NUnit Examples

### Simple pure function

```csharp
using NUnit.Framework;

[TestFixture]
public class RewardCalculatorTests
{
    [Test]
    public void CapDaily_ReturnsZero_WhenLimitNonPositive()
    {
        // Arrange
        var points = 100;
        var limit = 0;

        // Act
        var result = RewardCalculator.CapDaily(points, limit, DateTime.UtcNow);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
}
```

### Builder pattern for readability

```csharp
public class PlayerBuilder
{
    private int _points = 0;
    public PlayerBuilder WithPoints(int points) { _points = points; return this; }
    public Player Build() => new Player(_points);
}
```

```csharp
[Test]
public void Grant_AddsPoints_WhenUnderDailyCap()
{
    // Arrange
    var player = new PlayerBuilder().WithPoints(50).Build();
    var sut = new RewardService(limit: 100);

    // Act
    sut.Grant(player, 25);

    // Assert
    Assert.That(player.Points, Is.EqualTo(75));
}
```

## Test Smells and Fixes
- Slow tests: remove disk/network; substitute fakes.
- Brittle tests: assert on outcomes, not internals.
- Overuse of mocks: prefer fakes/stubs; mock only behavior contracts.
- Hidden dependencies: make them constructor parameters.

## Tooling
- NUnit (Unity compatible)
- Fluent assertions optional
- Coverage: measure lines/branches; target meaningful assertions, not just %

## Checklist
- Test names describe behavior
- No hidden time/randomness; use seams
- One focused assertion (or grouped asserts for one concept)
- Builders/fakes keep Arrange small
- Tests run fast locally
