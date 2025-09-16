# Feature Slice Checklist

A **slice** is a small, vertical piece of a feature that delivers a user‑ or
system‑visible outcome or passes a test.  Use this checklist to decide if
your slice is small enough.

## Definition of Small

* **Observable outcome**: The slice produces something you can see,
  measure or test (e.g. the timer pauses during silence, a JSON field is
  updated).  It is not just scaffolding.
* **Timebox**: Aim to complete a slice within a single working day (or
  less).  If it takes longer, split it further.
* **Single responsibility**: Each slice addresses one problem or capability
  from the feature strategy.  Avoid bundling multiple concerns.
* **Includes tests**: Write edit‑mode and/or play‑mode tests as
  appropriate.  A slice is only done when its tests pass.
* **Demo ready**: You should be able to demo the slice to a teammate or
  stakeholder.  For back‑end slices (e.g. JSON logging), demo by showing
  the log file or the test output.
* **Low risk**: A slice should not break existing functionality.  Use
  feature flags or configuration toggles when changing behaviour.

## Working with slices

1. **Plan using the strategy template**.  Identify your MVP slice and
   derive subsequent slices from it.
2. **Cross‑link to user stories and tasks**.  Each slice should map to a
   user story or a set of tasks from the epic planning prompts.
3. **Keep code review small**.  Small slices lead to small pull
   requests (≤ 400 lines changed) which are easier to review.
4. **Reflect on feedback**.  After completing a slice, review what
   worked and what didn’t, then adjust your next slice accordingly.

For examples of slicing VAD, strict mode and reward animations, see the
**feature strategy template** and the planning prompts in `.github/prompts/`.
