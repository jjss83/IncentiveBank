# Copilot Playbook for Unity Development

GitHub Copilot and similar LLMs can accelerate your work, but only if you
guide them well.  This playbook shows how to use Copilot effectively on
IncentiveBank.

## Understand Copilot’s strengths and weaknesses

Copilot excels at repetitive code, writing tests, debugging syntax and
explaining snippets. It is **not a
replacement** for your expertise. Always
validate generated code and ensure it aligns with the project’s goals and
constraints.

## Provide context

Copilot works best when you give it the right context:

* **Open relevant files** in your editor. Copilot uses the current buffer
  and nearby files as context.
* Include excerpts from the **GDD** or the **feature strategy template** at
  the top of a new file.  For example:

  ```csharp
  // Reading Rewards – Session timer
  // Goal: count seconds of voice until goal time; pause on silence
  // Strict mode requires caret movement and voice together
  // See docs/dev-guides/feature-strategy-template.md for constraints
  ```

* When using Copilot Chat, specify the file or symbol names to focus the
  assistant on the relevant code.

## Design before code

Before asking Copilot to generate code, write down the **problem**,
**constraints** and **acceptance criteria**.  Use the prompts below to
explore options:

```
// Ask Copilot for options
"Summarise the constraints from the GDD for the session timer and suggest
  two implementation strategies with trade‑offs."

// Explore vertical slice
"Propose the smallest vertical slice to implement VAD calibration and
  counting voice seconds, with acceptance tests."

// Delay code generation
"Generate NUnit tests for the slice; do not write the implementation yet."
```

Before generating or editing code, write a short, actionable todo list and keep it updated until the work is complete. This improves clarity and makes reviews easier.

- Create todos that reflect the smallest meaningful steps (3–7 items).
- Mark exactly one todo as in-progress at any time; complete it before starting the next.
- After implementation, revisit the list and mark all items done or document deferrals.

Template (inline in code or PR description):

```text
TODOS
- [ ] Add RewardCalculator.CapDaily tests (edge: limit <= 0)
- [ ] Implement CapDaily without allocations; update XML docs
- [ ] Add play-mode test for session timer coroutine
- [ ] Update ai-comments near hot path
```

See also: `ai-development-principles.md` and `unit-testing-best-practices.md`.


```
// Tests for session timer
"Write an edit‑mode test that verifies the timer only counts when
  `voiceDetected` is true.  Use a coroutine to simulate time passing."
```

Review the tests carefully and adjust them to ensure they capture the
expected behaviour.  Then implement the feature to make the tests pass.

## Diff‑aware refactoring

When refactoring, ask Copilot to explain the impact of a change:

```
"Summarise what changes if I extract this method into a helper class,
 and identify any potential side effects."
```

Review the diff and run your tests before accepting suggestions.

## Validate the output

Copilot can make mistakes.  Always validate its suggestions:

* **Understand the code** before committing【785259186302324†L399-L407】.  Ask
  Copilot Chat to explain the snippet if necessary.
* **Review suggestions carefully** – consider functionality,
  readability and maintainability【785259186302324†L404-L408】.
* Use **automated tests** and tools such as linting and static analysis to
  catch issues【785259186302324†L409-L410】.

## Consistency checks (required)

Always evaluate whether your change aligns with surrounding code. If not, choose the closest consistent approach and document the rationale inline (see `ai-comments-guide.md`).

Checklist:
- Naming matches existing modules (`ClassName.ai.md`)
- Patterns align (DI, ScriptableObjects, events, serialization)
- Testing approach matches neighbors (edit vs play mode, builders)
- Asset paths and folder organization follow Unity conventions

## Unity‑specific prompts

Unity introduces lifecycle and physics nuances.  Here are some useful prompts:

* "Explain the difference between `Update` and `FixedUpdate` and when to use
  each in the context of a VAD system."
* "Suggest a coroutine‑based approach for a reward pop animation lasting
  300 ms; discuss pros and cons compared to using an AnimationClip."
* "What pitfalls might occur when counting time with `Time.deltaTime` on
  mobile devices, and how can we mitigate them?"
* "How can we ensure that our session timer behaves consistently across
  Android and iOS?"

Use these as starting points.  Modify them to reflect your feature’s
constraints and the smallest slice you are working on.

## Links and references

- AI principles: `ai-development-principles.md`
- AI comments guide: `ai-comments-guide.md`
- Naming conventions: `ClassName.ai.md`
- Testing (general): `unit-testing-best-practices.md`
- Testing (Unity): `testing-in-unity.md`

## Summary

Copilot can be a powerful assistant if you guide it.  Provide context,
design first, generate tests, review suggestions and always keep your
project’s constraints in mind.  Link back to the **feature strategy
template** and **slice checklist** as you plan and implement your work.
