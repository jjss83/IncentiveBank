---
generated_at: 2025-09-11
source_gdd: GDDv1.1.md
workflow: kanban
---

# Kanban Backlog

## Summary
Scope: MVP vertical slice enabling a child to read a passage, have voice detected, time counted toward a goal, and receive a token reward with persistence
Current Focus: Pipeline validation on target devices (EP-00: splash screen build/deploy) → then Core reading + reward loop instrumentation and persistence
Out of Scope (Now): Strict mode phoneme-level ASR, advanced analytics, polish animations beyond token pop
WIP Policy: Max 1 active Code task per dev; Design tasks fast-tracked; Testing tasks paired

## Epics

### EP-00 Pipeline Validation & Device Bring-up

Narrative: A minimal app runs on all target devices and displays a splash image to validate build, signing, and deployment pipelines
Business Value: De-risks the project by proving end-to-end CI/CD and device compatibility early, enabling faster feedback on real hardware
Status: Active

### EP-01 Core Reading & Timing

Narrative: Child can open a passage and system counts valid reading time using voice detection until goal reached
Business Value: Delivers fundamental loop proving engagement and timing accuracy
Status: Active

### EP-02 Rewards & Persistence

Narrative: Reward tokens granted on goal completion and persisted locally for future motivation
Business Value: Reinforces habit formation and validates incentive banking concept
Status: Active

### EP-03 Content & Localization

Narrative: Passages and UI available in EN & ES with simple difficulty grouping
Business Value: Extends reach to bilingual households and supports early usability tests
Status: Active

### EP-04 Strict Mode (Gated)

Narrative: Optional stricter enforcement combining caret movement with voice for time counting
Business Value: Provides pathway for higher rigor sessions and future differentiation
Status: Queued

## User Stories

### US-001 (Epic: EP-01)

As a child, I want reading time to count only while I'm actually speaking so that rewards feel fair
Acceptance Criteria:

- Time counted only when voice present matches VAD thresholds
- Timer pauses during silence automatically
- Session ends manually or on goal completion
- Edge noise (TV/music) minimally inflates counted seconds
Status: Ready

## Tasks

### TK-0001 US-001 design doc (Story: US-001, Type: Design, Est: S)

Outcome: Lightweight design describing VAD pipeline, timer state machine, update loop responsibilities
GDD Trace: GDDv1.1.md#vad-voice-activity-detection
Dependencies: None
Acceptance Criteria:

- File `Documents/design/US-001-design.md` created
- Sections: Purpose, Components, Data Flow, State Machine, Risks, Decisions
- At least 2 risks + 1 open question captured

### TK-0002 VAD service processes mic frames (Story: US-001, Type: Code, Est: M)

Outcome: `VADService` converts PCM frames to voice active/inactive events with hysteresis
GDD Trace: GDDv1.1.md#vad-voice-activity-detection
Dependencies: TK-0001
Acceptance Criteria:

- File `Assets/Scripts/Audio/VADService.cs` created
- Public event `OnVoiceActive(bool active)` fires on state changes only
- Hysteresis thresholds configurable via serialized fields
- Debounce window 600ms implemented per GDD spec
- No per-frame allocations over 5k frames test harness

### TK-0003 Reading session timer counts active voice seconds (Story: US-001, Type: Code, Est: S)

Outcome: `ReadingSessionTimer` tracks counted seconds only while voice active
GDD Trace: GDDv1.1.md#core-loops-compact
Dependencies: TK-0001
Acceptance Criteria:

- File `Assets/Scripts/Session/ReadingSessionTimer.cs` created
- Method `Update(float dt)` only adds dt when active flag true
- Drift <0.1s over simulated 5 minutes test
- Exposes `CountedSeconds` read-only property
- Zero GC allocs after warmup (100 frames)

### TK-0004 Session controller integrates VAD and timer (Story: US-001, Type: Code, Est: S)

Outcome: `SessionController` orchestrates mic start, VAD subscription, timer update, goal check
GDD Trace: GDDv1.1.md#core-loops-compact
Dependencies: TK-0002, TK-0003
Acceptance Criteria:

- File `Assets/Scripts/Session/SessionController.cs` created
- Public `StartSession(passageId)` initializes timer and VAD
- Triggers event `OnGoalReached` exactly once
- Handles manual end without awarding token
- Gracefully logs warning if mic unavailable

### TK-0005 Noise resilience spectral flatness option (Story: US-001, Type: Code, Est: S)

Outcome: Optional spectral flatness gating reduces music/TV false positives
GDD Trace: GDDv1.1.md#vad-voice-activity-detection
Dependencies: TK-0002
Acceptance Criteria:

- Feature flag exposed in `settings.json` (`useSpectralFlatness`)
- Flatness calculation window adjustable (config)
- Adds <2% CPU on mid-tier device (profiling note placeholder)
- Disabled path yields identical output to baseline
- Unit test simulates tone vs speech classification

### TK-0016 Settings file reader/writer (Story: US-001, Type: Code, Est: S)

Outcome: `SettingsStore` loads and persists settings.json values
GDD Trace: GDDv1.1.md#settings--logs-json-locked
Dependencies: TK-0001
Acceptance Criteria:

- File `Assets/Scripts/Config/SettingsStore.cs` created
- Provides strongly typed accessors for settings
- Uses default values when file missing
- Persists modifications on demand without blocking main thread
- Unit test covers missing file fallback

### TK-0018 Minimal reading view UI layout (Story: US-001, Type: UX, Est: S)

Outcome: Basic Reading View displaying passage text, timer, voice indicator, end session button
GDD Trace: GDDv1.1.md#uxui-lean
Dependencies: TK-0004
Acceptance Criteria:

- Prefab `ReadingView.prefab` exists
- Timer updates visually at least 2Hz
- Voice indicator toggles within 150ms of state change
- End session button triggers controller stop
- Layout scales legibly on tablet and small laptop

## Parking Lot

- Token animation polish easing variants (GDD trace: rewards)
- Advanced noise classification ML (needs new heading proposal)
- Analytics events export (needs heading proposal)

## Risks

- VAD false positives create unfair rewards — Mitigation: spectral flatness + debounce + manual QA tuning
- File IO latency on some devices — Mitigation: async writes and small append chunks
- Locale asset drift (missing translations) — Mitigation: fallback to `en` with log warning

## Assumptions

- Unity version supports required microphone APIs equally across target platforms
- Device microphones provide sufficient RMS dynamic range without preamp gain adjustments

## Conventions

- Branch pattern: feat/{task-id}-{slug}
- Labels suggestion: type:{type}, size:{XS|S|M}
- Status field used in file maps to board columns

## Flow Metrics Hints

- Cycle Time: Start = first status change to In Progress, End = Done
- Throughput: Done tasks per calendar week (exclude design optionally)
- Aging WIP: Highlight tasks >3 days In Progress
