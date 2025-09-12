---
iteration: 1
generated_at: 2025-09-11
source_gdd: GDDv1.1.md
---

# Iteration 1 Plan

## Summary
Scope: Deliver a vertical slice: calibrate mic, detect voice, time a reading session, grant token reward, load localized content, basic strict mode gating, and persist logs/settings
Out of scope: Advanced ASR/phoneme features, multi-session analytics dashboards, polish & accessibility extras

## Epics
### EP1-01 Reading Session Core
Narrative: Enable a child to perform a full reading session from calibration through reward using a single passage
Business Value: Establishes the foundational interactive loop proving viability of core mechanic

### EP1-02 Content & Localization Loading
Narrative: Provide bilingual (EN/ES) passage and UI text loading from local JSON manifest and files
Business Value: Validates bilingual promise and data-driven approach early, reducing integration risk

### EP1-03 Rewards & Progress Persistence
Narrative: Track session progress, grant tokens on goal, and persist cumulative totals locally
Business Value: Demonstrates incentive loop motivation and verifies data persistence reliability

### EP1-04 Strict Mode Foundations
Narrative: Introduce a minimal strict mode that requires both voice and caret movement cadence to count time
Business Value: Derisks stricter engagement mechanic early without full ASR complexity

## User Stories
#### US1-01 (Epic: EP1-01)
As a reader, I want the app to detect my voice and time my active reading so that I earn progress only when I read aloud
Acceptance Criteria:
- Voice presence increases session active seconds
- Silence pauses counting within 600ms debounce window
- Calibration sets noise floor before counting begins
- Timer visible updates at least twice per second

#### US1-02 (Epic: EP1-02)
As a reader, I want to browse and load passages in my chosen language so that I can read appropriate localized content
Acceptance Criteria:
- Manifest loads locales and groups from JSON
- Passage list shows titles for selected locale
- Selecting passage loads body text into reading view
- Switching locale refreshes passage list

#### US1-03 (Epic: EP1-03)
As a reader, I want to receive a token reward when I reach the goal time so that I feel motivated to continue reading
Acceptance Criteria:
- Goal reached triggers token increment
- Token pop animation plays once on reward
- Updated token total persists to logs store
- Session entry recorded with goalMet flag

#### US1-04 (Epic: EP1-04)
As a parent enabling strict mode, I want reading time to count only with both voice and caret movement so that engagement with text is ensured
Acceptance Criteria:
- Strict mode toggle flag read from settings
- Time counts only with voice + cadence satisfied
- Missing cadence for >3s suspends counting
- UI indicates strict gating state change

## Tasks
### IT1-001 US1-01 design doc (Story: US1-01, Type: Design, Est: S)
Outcome: Lightweight design document describing VAD calibration, frame processing, timer integration
GDD Trace: GDDv1.1.md#vad-voice-activity-detection
Dependencies: None
Acceptance Criteria:
- File `Documents/design/US1-01-design.md` created
- Sections: Purpose, Scope, Data Flow, Key Classes, Risks, Decisions
- At least 1 risk + 1 open question captured

### IT1-002 VAD processor counts active frames (Story: US1-01, Type: Code, Est: M)
Outcome: `VadProcessor` converts audio samples to voice-active boolean with hysteresis & debounce
GDD Trace: GDDv1.1.md#vad-voice-activity-detection
Dependencies: IT1-001
Acceptance Criteria:
- Class `VadProcessor` created at `Assets/Scripts/Audio/VadProcessor.cs`
- Methods: Initialize(noiseFloorFrames), ProcessFrame(float[] samples)
- Debounce prevents flips <600ms
- Per-frame allocations = 0 after warmup
- Unit test simulates enter/exit thresholds

### IT1-003 Calibration routine establishes noise floor (Story: US1-01, Type: Code, Est: S)
Outcome: `MicCalibrator` samples ambient frames and computes baseline RMS
GDD Trace: GDDv1.1.md#vad-voice-activity-detection
Dependencies: IT1-001
Acceptance Criteria:
- Class `MicCalibrator` at `Assets/Scripts/Audio/MicCalibrator.cs`
- Collects ≥ 1s ambient audio
- Exposes property NoiseFloorRms
- Returns success flag if variance within tolerance
- Logs warning if unstable floor

### IT1-004 Session timer accumulates active seconds (Story: US1-01, Type: Code, Est: S)
Outcome: `SessionTimer` adds delta only when voiceActive flag true
GDD Trace: GDDv1.1.md#core-loops-compact
Dependencies: IT1-002
Acceptance Criteria:
- Class `SessionTimer` at `Assets/Scripts/Session/SessionTimer.cs`
- Public Start(), Stop(), Reset(), Update(delta, active)
- Drift <0.1s over simulated 5 minutes
- No GC allocs in Update in profiler sample
- Exposes GoalReached event

### IT1-005 Reading view displays live timer and voice indicator (Story: US1-01, Type: UX, Est: S)
Outcome: UI shows active seconds and voice-detected indicator updating
GDD Trace: GDDv1.1.md#reading-view
Dependencies: IT1-004
Acceptance Criteria:
- Prefab `Assets/Prefabs/ReadingView.prefab` created
- Timer label updates ≥2x per second
- Voice indicator toggles within 200ms of state change
- Uses TextMeshPro components
- No missing reference warnings in console

### IT1-006 US1-02 design doc (Story: US1-02, Type: Design, Est: S)
Outcome: Design document for content manifest loading, passage selection, localization handling
GDD Trace: GDDv1.1.md#content--localization-final
Dependencies: None
Acceptance Criteria:
- File `Documents/design/US1-02-design.md` created
- Includes JSON schema notes for manifest & passages
- Maps classes: ContentManifestLoader, PassageRepository
- At least 1 risk + 1 open question captured

### IT1-007 Manifest loader parses groups (Story: US1-02, Type: Code, Est: S)
Outcome: `ContentManifestLoader` reads manifest JSON into model
GDD Trace: GDDv1.1.md#content--localization-final
Dependencies: IT1-006
Acceptance Criteria:
- File `Assets/Scripts/Content/ContentManifestLoader.cs` exists
- Loads locales and groups arrays
- Gracefully handles missing file with error log
- Returns strongly typed structure
- Unit test: missing file returns null model

### IT1-008 Passage repository loads passage text (Story: US1-02, Type: Code, Est: S)
Outcome: `PassageRepository` loads passage JSON and provides line-split body
GDD Trace: GDDv1.1.md#content--localization-final
Dependencies: IT1-006
Acceptance Criteria:
- File `Assets/Scripts/Content/PassageRepository.cs` exists
- Provides GetPassagesByLocale(locale)
- Splits body by newline if present
- Null wordBoundaries triggers whitespace split path note
- Unit test validates English sample

### IT1-009 Passage list UI shows localized titles (Story: US1-02, Type: UX, Est: S)
Outcome: UI lists passage titles for active locale and refreshes on change
GDD Trace: GDDv1.1.md#passage-select
Dependencies: IT1-007
Acceptance Criteria:
- Prefab `Assets/Prefabs/PassageListView.prefab` exists
- Displays at least 3 sample items with scrolling
- Locale switch triggers refresh within 300ms
- Selecting item raises PassageSelected event
- No duplicate titles shown

### IT1-010 US1-03 design doc (Story: US1-03, Type: Design, Est: S)
Outcome: Design doc for reward detection, token update, persistence logging
GDD Trace: GDDv1.1.md#rewards
Dependencies: None
Acceptance Criteria:
- File `Documents/design/US1-03-design.md` created
- Describes token grant flow sequence
- Identifies classes: RewardService, LogStore
- At least 1 risk + 1 open question captured

### IT1-011 Reward service grants tokens on goal (Story: US1-03, Type: Code, Est: S)
Outcome: `RewardService` listens to GoalReached and updates token total
GDD Trace: GDDv1.1.md#rewards
Dependencies: IT1-010
Acceptance Criteria:
- File `Assets/Scripts/Rewards/RewardService.cs` exists
- Subscribes to SessionTimer GoalReached event
- Increments tokens by configured amount
- Prevents duplicate grant in same session
- Emits RewardGranted event

### IT1-012 Token pop animation plays (Story: US1-03, Type: UseAsset, Est: S)
Outcome: Visual token pop effect triggers once on reward
GDD Trace: GDDv1.1.md#rewards
Dependencies: IT1-011
Acceptance Criteria:
- Prefab `Assets/Prefabs/TokenPop.prefab` exists
- Animation duration 200–400ms
- Plays exactly once per reward event
- Scales up then eases back to default
- No warnings in animation console

### IT1-013 Logs store persists session data (Story: US1-03, Type: Code, Est: S)
Outcome: `LogStore` appends session entries and saves token total
GDD Trace: GDDv1.1.md#settings--logs-json-locked
Dependencies: IT1-010
Acceptance Criteria:
- File `Assets/Scripts/Persistence/LogStore.cs` exists
- Method AppendSession(entry) writes to `logs.json`
- File created if absent with initial structure
- Handles IO exception with warning not crash
- Unit test verifies goalMet true entry

### IT1-014 Settings loader provides config values (Story: US1-03, Type: Code, Est: S)
Outcome: `SettingsLoader` loads settings JSON into struct used by services
GDD Trace: GDDv1.1.md#settings--logs-json-locked
Dependencies: IT1-010
Acceptance Criteria:
- File `Assets/Scripts/Persistence/SettingsLoader.cs` exists
- Loads locale, sessionGoalMinutes, rewardTokensPerGoal
- Defaults applied if fields missing
- Provides in-memory immutable snapshot
- Unit test missing optional field applies default

### IT1-015 US1-04 design doc (Story: US1-04, Type: Design, Est: S)
Outcome: Design doc for strict mode cadence + voice gating integration
GDD Trace: GDDv1.1.md#caretstrictness
Dependencies: None
Acceptance Criteria:
- File `Documents/design/US1-04-design.md` created
- Defines cadence threshold (≥1 word per 3s)
- Describes state machine for gating
- At least 1 risk + 1 open question captured

### IT1-016 Caret tracker reports word advancement cadence (Story: US1-04, Type: Code, Est: S)
Outcome: `CaretTracker` measures elapsed time since last word index increment
GDD Trace: GDDv1.1.md#caretstrictness
Dependencies: IT1-015
Acceptance Criteria:
- File `Assets/Scripts/Reading/CaretTracker.cs` exists
- Method RegisterAdvance() updates timestamp
- Property CadenceSatisfied true if ≥1 advance/3s
- No GC allocs per frame in update path
- Unit test simulates advances and idle period

### IT1-017 Strict gating integrates voice + caret (Story: US1-04, Type: Code, Est: S)
Outcome: `StrictModeGate` exposes IsCounting combining voice and cadence
GDD Trace: GDDv1.1.md#caretstrictness
Dependencies: IT1-016
Acceptance Criteria:
- File `Assets/Scripts/Reading/StrictModeGate.cs` exists
- Method Evaluate(voiceActive, cadenceSatisfied) returns bool
- Suspends counting if cadence false >3s
- Logs state transitions (enter/exit counting)
- Unit test covers voice only, cadence only, both

## Parking Lot
- Advanced spectral flatness optimization (GDD trace: vad-voice-activity-detection)
- Token streak or bonus system (GDD trace: rewards)
- Accessibility font size toggle (GDD trace: uxui-lean)

## Risks
- Risk: False positives from ambient speech — Mitigation: add spectral flatness check stub early
- Risk: JSON IO blocking main thread — Mitigation: small async wrapper or batching writes
- Risk: Cadence threshold frustrates early readers — Mitigation: adjustable threshold constant

## Assumptions
- Single active session at a time
- Sample scene `SampleScene.unity` serves as integration testbed
- JSON files reside under `Application.persistentDataPath/AppData` in runtime

## Conventions
- Branch pattern: feat/<task-id>-<slug>
- Labels suggestion: type:<Type>, iter:1, size:<XS|S|M>
- Namespace root: ReadingRewards
- Unit test naming: ClassNameTests.cs
