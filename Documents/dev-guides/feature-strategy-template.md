# Feature Strategy Template

Use this template to capture the **design and planning** of a new feature
*before* writing code.  Copy it into an issue or document and fill in the
sections.  Keeping strategy explicit helps the team agree on scope,
constraints and acceptance criteria.

## Problem

Describe the problem or opportunity in one or two sentences.  Focus on
why this feature matters to our players (e.g. “Kids get confused when
the timer keeps running during silence; we need clear feedback.”).

## Goals / Non‑Goals

List what this feature will achieve and what it intentionally will not.
Bullets are ideal:

* Goal: Count only **voice‑present** seconds toward the session goal.
* Goal: Show a subtle timer chip that pauses when no voice is detected.
* Non‑goal: Save audio recordings (out of scope for MVP).

## Constraints

Include any technical, platform or design constraints, such as:

* Must run offline and store data locally in JSON.
* Use proven VAD methods (RMS, hysteresis) without per‑frame allocations.
* Support Android, iOS and Windows; handle microphone selection via
  `Microphone.devices`.
* Follow the GDD’s acceptance criteria.

## Risks

Identify potential issues and proposed mitigations.  Example risks:

* **False positives from background noise** – mitigate with spectral
  flatness and debounce thresholds.
* **Strict mode fatigue** – allow slower caret cadence in strict mode.
* **Unity beta instability** – fall back to latest LTS if blocking bugs.

## Acceptance Criteria

Write objective, testable bullet points describing what must be true after
the feature is complete.  Avoid “and/or” and trailing punctuation.

* Timer counts only while `voiceDetected` is true.
* In strict mode, timer counts only when both `voiceDetected` and
  `caretMovedWithin` (e.g. 3 s) are satisfied.
* When goal time is reached, reward tokens are granted and persisted to
  `logs.json`.
* Content loads correctly from the manifest and localisation strings are
  displayed based on the selected locale.

## MVP Slice

Describe the smallest vertical slice that delivers value.  Example:

* Implement VAD calibration and voice counting with a placeholder UI.
* Pause the timer during silence; no strict mode yet.
* Log sessions in JSON.

## Small Tasks List

Break the MVP slice into small, actionable tasks (XS or S size).  Each
should fit comfortably in a day or less and produce an observable result.
Link tasks to user stories when applicable.

1. **[Design]** Define RMS thresholds and debouncing constants in a
   ScriptableObject.
2. **[Config]** Read microphone settings from JSON and select the default device.
3. **[Code]** Implement VAD calibration and voice counting using a
   coroutine in a dedicated MonoBehaviour.
4. **[Test]** Write edit‑mode and play‑mode tests to verify counting logic.

## Test Plan

Summarise how you will validate the feature.  Reference the
`testing‑in‑unity.md` guide for guidance on edit vs. play mode tests.  For
instance:

* Use edit‑mode NUnit tests to verify VAD threshold logic with synthetic
  data.
* Use play‑mode tests to simulate sessions and ensure tokens are awarded
  correctly.
* Manually verify the timer pauses during silence on Android and iPad.

## Metrics / Telemetry

Describe any instrumentation needed.  What logs, counters or events will
track feature success?  Example:

* Log `secondsCounted` and `goalMet` per session in `logs.json`.
* Track number of strict‑mode sessions vs. early/older sessions.

## Rollback Strategy

Explain how to disable or revert the feature if issues arise.  For
example, add a `strictMode` toggle in `settings.json` to disable strict
counting.
