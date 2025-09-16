# Testing in Unity

Automated tests build confidence and shorten feedback loops.  Unity’s Test
Framework supports two modes: **Edit‑mode tests** and **Play‑mode tests**.
This guide summarises when to use each and provides a starting recipe.

## Edit‑mode vs. Play‑mode tests

* **Edit‑mode tests** run inside the Unity Editor and have access to
  editor code【504304711547036†L21-L31】.  Use them to test pure logic and
  editor extensions, such as VAD threshold calculations or JSON parsing.
* **Play‑mode tests** run either in the Editor’s Play Mode or in a
  standalone player【504304711547036†L44-L48】.  They execute your game
  code and can exercise coroutines, scenes and physics.  Use them to
  simulate reading sessions, strict mode behaviour or reward animations.

### Choosing test attributes

* Use NUnit’s `Test` attribute for synchronous tests.  It works in both
  edit and play modes and is simpler to write【504304711547036†L65-L71】.
* Use `UnityTest` for coroutine tests that need to yield, wait for a frame
  or a delay.  This is common in play‑mode tests when simulating time.

### Assembly definitions

Place your test scripts in a separate folder with an `.asmdef` file that
references `nunit.framework` and, for play‑mode tests, your game
assembly.  See the examples in the Unity Test Framework documentation
【504304711547036†L51-L62】.

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

## Deterministic seeds

When your tests depend on randomness (e.g. randomised passages), set
deterministic seeds using `Random.InitState(seed)` to ensure repeatable
results.  Avoid using frame‑dependent values as seeds.

## Running tests in CI

Configure your CI (e.g. a GitHub Actions workflow) to run edit and
play‑mode tests on each pull request.  This ensures that regressions are
caught early.  See the `commit‑and‑pr‑guide.md` for PR expectations.

Testing is an investment; start small and build a habit.  For more on
slicing tests with vertical slices, see the **feature slice checklist**.
