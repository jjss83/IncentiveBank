# Testing in Unity

Automated tests build confidence and shorten feedback loops. Unity’s Test
Framework supports two modes: **Edit‑mode tests** and **Play‑mode tests**.
This guide summarises when to use each and provides actionable templates and
best practices aligned with our AI‑enhanced development principles.

## Edit‑mode vs. Play‑mode tests

* **Edit‑mode tests** run inside the Unity Editor and have access to
   editor code. Use them to test pure logic and
   editor extensions, such as VAD threshold calculations or JSON parsing.
* **Play‑mode tests** run either in the Editor’s Play Mode or in a
   standalone player. They execute your game
   code and can exercise coroutines, scenes and physics. Use them to
   simulate reading sessions, strict mode behaviour or reward animations.

### Choosing test attributes

* Use NUnit’s `Test` attribute for synchronous tests. It works in both
   edit and play modes and is simpler to write.
* Use `UnityTest` for coroutine tests that need to yield, wait for a frame
   or a delay. This is common in play‑mode tests when simulating time.

### Assembly definitions

Place your test scripts in a separate folder with an `.asmdef` file that
references `nunit.framework` and, for play‑mode tests, your game
assembly. See the examples in the Unity Test Framework documentation.

## First test recipe

Here is a simple recipe to write your first test for a new slice:

1. **Identify the behaviour** you want to verify from the acceptance
   criteria (e.g. “the timer counts only when voice is detected”).
2. **Decide on the mode**.  For logic that doesn’t rely on scenes or
   time, write an edit‑mode test.  For behaviour involving coroutines,
   delays or the Unity player loop, use a play‑mode test.
3. **Write a failing test**.  Create a new `public class` in the
   appropriate test folder.  Use `Assert` statements to define what
   constitutes success.

   ```csharp
   using NUnit.Framework;

   public class TimerTests
   {
       [Test]
       public void CountsTimeOnlyWhenVoicePresent()
       {
           var timer = new SessionTimer();
           timer.VoiceDetected = false;
           timer.Tick(1f);
           Assert.AreEqual(0f, timer.TimeCounted);

           timer.VoiceDetected = true;
           timer.Tick(1f);
           Assert.AreEqual(1f, timer.TimeCounted);
       }
   }
   ```

4. **Run the test** via the Test Runner (Window → General → Test Runner).
   It should fail until you implement the logic in `SessionTimer`.
5. **Implement just enough code** to make the test pass.  Avoid writing
   extra functionality.
6. **Refactor and add more tests** for other behaviours (e.g. strict mode
   requiring caret movement within a time window).

## Quick start templates

Edit‑mode (pure logic):

```csharp
using NUnit.Framework;

[TestFixture]
public class RewardCalculatorTests
{
   /// AI: Purpose: Validate clamping logic under boundary conditions.
   [Test]
   public void CapDaily_ReturnsZero_WhenLimitIsNonPositive()
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

Play‑mode (coroutine and time):

```csharp
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

public class SessionTimerPlayModeTests
{
   /// AI: Purpose: Counts only while voice is detected over multiple frames.
   [UnityTest]
   public IEnumerator CountsOnlyWhenVoiceDetected()
   {
      var timer = new SessionTimer();
      timer.VoiceDetected = false;
      yield return null; // simulate a frame
      Assert.That(timer.TimeCounted, Is.EqualTo(0f));

      timer.VoiceDetected = true;
      yield return null; // simulate a frame
      Assert.That(timer.TimeCounted, Is.GreaterThan(0f));
   }
}
```

## Best practices

- Follow AAA (Arrange‑Act‑Assert); test one behavior at a time.
- Prefer deterministic tests; inject clocks/random sources.
- Keep scene dependencies out of edit‑mode tests; use play‑mode for scene/physics.
- Use builders/fakes to keep setup concise.
- Name tests as `Type_Scenario_Expectation`.
- Add small AI‑specific comments to clarify tricky constraints or invariants.

## Deterministic seeds

When your tests depend on randomness (e.g. randomised passages), set
deterministic seeds using `Random.InitState(seed)` to ensure repeatable
results.  Avoid using frame‑dependent values as seeds.

## Common pitfalls

- Using `Time.time` in edit‑mode tests: avoid; simulate via parameters.
- Relying on `yield return new WaitForSeconds` for logic validation: prefer advancing frames and injecting clocks.
- Hidden allocations in hot paths: assert GC behavior in performance tests where relevant.

## Organization and asmdefs

- Place edit‑mode tests under `Tests/EditMode` with their own `.asmdef`.
- Place play‑mode tests under `Tests/PlayMode` with their own `.asmdef`.
- Reference `nunit.framework` and the runtime assembly you are testing.
- Mirror folder structure of runtime code to ease navigation.

## Running tests in CI

Configure your CI (e.g. a GitHub Actions workflow) to run edit and
play‑mode tests on each pull request.  This ensures that regressions are
caught early.  See the `commit‑and‑pr‑guide.md` for PR expectations.

Testing is an investment; start small and build a habit. For more on
slicing tests with vertical slices, see the **feature slice checklist**.

## Checklist

- Chosen the appropriate mode (edit vs play)
- Deterministic and isolated (no hidden time/randomness)
- Clear naming and AAA structure
- Minimal setup using builders/fakes
- Inline AI comments for non‑obvious constraints

## References

- Unity Test Framework: https://docs.unity3d.com/Packages/com.unity.test-framework@latest
- NUnit: https://docs.nunit.org/
- Our AI principles: `ai-development-principles.md`
