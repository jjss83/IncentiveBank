---
iteration: 1
generated_at: 2025-09-11
source_gdd: GDDv1.1.md
---

# Iteration 1 Plan

## Summary

Scope: Deliver a functional offline reading loop with voice-based timing, token reward, and configurable settings/content.

Out of scope: Strict mode caret cadence enforcement, Spanish content breadth, polished token animation, advanced noise classification.

## Epics

### EP1-01 Core Reading Loop
Narrative: Establish the minimum loop (detect voice, accumulate time, reward tokens) enabling a kid to complete a session.
Business Value: Validates core engagement mechanic and ensures fair reward timing.

### EP1-02 Content & Configuration
Narrative: Provide JSON-driven settings and passages so parents can adjust goals and content without builds.
Business Value: Supports iteration and localization readiness.

## User Stories

### US1-01 (Epic: EP1-01)
As a reader, I want the timer to count only while I'm speaking so that earning tokens feels fair.
Acceptance Criteria:

- Voice indicator visible while speaking
- Timer halts within 0.5s of silence
- No perceptible stutter from detection logic

### US1-02 (Epic: EP1-01)
As a reader, I want to receive a token when I meet my reading goal so that I feel rewarded for persistence.
Acceptance Criteria:

- Single token grant when elapsed >= goal
- Visual feedback within 0.5s
- Session log contains tokensAwarded

### US1-03 (Epic: EP1-02)
As a parent, I want to edit a settings JSON file to adjust session length so that I can tailor difficulty.
Acceptance Criteria:

- Changing sessionGoalMinutes updates next launch
- Invalid JSON falls back to defaults with warning

## Tasks

### IT1-001 Mic calibration service (Story: US1-01, Type: Code, Est: S)
Outcome: `MicCalibrationService` samples ambient RMS for <=3s and exposes noise floor.
GDD Trace: GDDv1.1.md#VAD-voice-activity-detection
Dependencies: None
Acceptance Criteria:

- Class at Assets/Scripts/Audio/MicCalibrationService.cs
- Calibrate() async method returns floor value
- Floor stable ±5% over two consecutive runs
- No allocations after first run

### IT1-002 Voice activity detector (Story: US1-01, Type: Code, Est: M)
Outcome: `VoiceActivityDetector` outputs VoiceActive flag using hysteresis + 600ms debounce.
GDD Trace: GDDv1.1.md#VAD-voice-activity-detection
Dependencies: IT1-001
Acceptance Criteria:

- Script at Assets/Scripts/Audio/VoiceActivityDetector.cs
- Inspector fields: EnterThreshold, ExitThreshold, DebounceMs
- Flicker-free at threshold (no more than 1 false toggle per 10s near boundary)
- Zero GC alloc per frame (5k frame sample)

### IT1-003 Reading session timer (Story: US1-01, Type: Code, Est: S)
Outcome: Aggregates active voice seconds; exposes progress & goal.
GDD Trace: GDDv1.1.md#Core-Loops
Dependencies: IT1-002
Acceptance Criteria:

- Class ReadingSessionTimer with Start/Reset/GetElapsedSeconds
- Only increments while VoiceActive true
- Drift <0.1s over simulated 300s
- Public event OnGoalReached fired once

### IT1-004 Token grant logic (Story: US1-02, Type: Code, Est: S)
Outcome: Awards tokens via Single Grant when timer reaches goal.
GDD Trace: GDDv1.1.md#Rewards
Dependencies: IT1-003
Acceptance Criteria:

- Method TryGrantToken(elapsedSeconds)
- Returns true only first time condition met
- Emits OnTokenGranted(int amount)
- Unit test stub documented (if not implemented)

### IT1-005 Token pop feedback placeholder (Story: US1-02, Type: UX, Est: XS)
Outcome: Pulse animation on token icon upon grant.
GDD Trace: GDDv1.1.md#Rewards
Dependencies: IT1-004
Acceptance Criteria:

- Animation clip Assets/Animations/TokenPop.anim
- Triggered by subscribing to OnTokenGranted
- Duration 0.2–0.4s ease-out
- No console warnings

### IT1-006 Persist logs.json entry (Story: US1-02, Type: Config, Est: S)
Outcome: Appends session record after token grant.
GDD Trace: GDDv1.1.md#Settings-Logs-JSON-locked
Dependencies: IT1-004
Acceptance Criteria:

- File path uses settings contentRoot/logs.json
- Array append preserves existing entries
- tokensAwarded matches grant
- IO failure logs single warning

### IT1-007 Load settings.json (Story: US1-03, Type: Config, Est: S)
Outcome: Loads or creates settings with defaults.
GDD Trace: GDDv1.1.md#Settings-Logs-JSON-locked
Dependencies: None
Acceptance Criteria:

- Reads before gameplay systems initialize
- Missing file triggers creation with defaults
- Invalid JSON triggers warning + defaults
- Provides immutable Settings object

### IT1-008 Load manifest & passage (Story: US1-03, Type: Code, Est: S)
Outcome: Loads manifest groups and first passage for display.
GDD Trace: GDDv1.1.md#Content-Localization-final
Dependencies: IT1-007
Acceptance Criteria:

- Manifest groups count >0
- At least one passage body non-empty
- Missing file logs warning only
- API GetPassages(locale) returns filtered list

### IT1-009 Reading View minimal UI (Story: US1-01, Type: UX, Est: M)
Outcome: Displays passage text, voice indicator, timer progress.
GDD Trace: GDDv1.1.md#UX-UI-lean
Dependencies: IT1-003, IT1-008
Acceptance Criteria:

- TextMeshPro renders passage with line breaks
- Voice indicator toggles <300ms after change
- Timer ticks each second
- End Session button placeholder present

### IT1-010 Home screen + navigation (Story: US1-02, Type: UX, Est: S)
Outcome: Landing screen shows token total and launches reading flow.
GDD Trace: GDDv1.1.md#UX-UI-lean
Dependencies: IT1-004, IT1-006
Acceptance Criteria:

- Token total updates after a completed session
- Start Reading navigates to reading UI
- Return path disposes temporary objects
- No missing references in console

## Parking Lot

- Strict mode cadence enforcement (future story)
- Full spectral flatness filter implementation
- Token pop polish (particles, easing)

## Risks

- VAD false positives under TV/music — Mitigation: Evaluate spectral heuristic early if >5% misclassification
- File IO spikes on low-end devices — Mitigation: Lazy flush & buffered writes

## Assumptions

- One English passage enough to validate loop
- Single combined scene acceptable (UI panels swapping)

## Conventions

- ID prefixes: EP / US / IT
- Branch pattern: feat/`id`-`slug`
- Suggested labels: epic, story, task, `iter:1`, `type:<Type>`, `size:<Est>`, `story:US1-0X`, `epic:EP1-01`
