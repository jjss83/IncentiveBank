# Pull Request Template

<!--
Use this template to ensure every pull request provides enough context
for reviewers.  Delete any sections that are not applicable.
-->

## Problem

What problem does this PR solve?  Reference the relevant epic/user story
or task (e.g. `EP-01`, `US-005`, `TK-0007`).

## Goals and Non‑Goals

Summarise the goals of this change and note anything intentionally left
out (non‑goals).

## Slice / MVP

Describe the vertical slice implemented.  What user‑ or system‑visible
outcome does it deliver?  Link to the feature strategy if applicable.

## Implementation Notes

Briefly explain the approach.  Highlight any trade‑offs, patterns or
platform checks used.  Note if you consulted the Unity basics or Copilot
playbook for this implementation.

## Tests Added

List the tests you added or updated.  Include both edit‑mode and
play‑mode tests where appropriate.  If no tests were added, explain why.

## Manual Verification

Describe how you manually verified the change (e.g. tested on Android
tablet, ensured the timer pauses during silence, etc.).  Include
screenshots, GIFs or logs if visual changes were made.

## Checklist

- [ ] Acceptance criteria from the feature strategy are met
- [ ] All tests pass locally and in CI
- [ ] Unity opens cleanly; no new warnings
- [ ] Documentation updated if needed
- [ ] Linked to relevant epics/stories/tasks

---

Need help?  Refer to the [developer guides](../docs/dev-guides/README.md) for
guidance on planning, slicing, testing and reviewing code.
