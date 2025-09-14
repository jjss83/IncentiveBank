---
generated_at: 2025-09-11
source_gdd: GDDv1.1.md
workflow: kanban
---

# Kanban Backlog

## Summary
Scope: MVP vertical slice enabling a child to read a passage, have voice detected, time counted toward a goal, and receive a token reward with persistence
Current Focus: Core reading + reward loop instrumentation and persistence
Out of Scope (Now): Strict mode phoneme-level ASR, advanced analytics, polish animations beyond token pop
WIP Policy: Max 1 active Code task per dev; Design tasks fast-tracked; Testing tasks paired

## Epics
### EP-00 HelloWorld Platform Validation
Narrative: Validate basic Unity app launch and splash screen display on Android, iPad, and Windows.
Business Value: Ensures build pipeline and device compatibility before deeper feature work.
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
#### US-000 (Epic: EP-00)
As a developer, I want to see a splash screen on all target platforms so that I know the build and launch process works.
Acceptance Criteria:
- App launches and displays splash screen on Android, iPad, Windows
- No errors or crashes on startup
- Splash screen visible for at least 1 second
- Build instructions documented for each platform
Status: Ready

### US-001 (Epic: EP-01)
As a child, I want reading time to count only while I'm actually speaking so that rewards feel fair
Acceptance Criteria:

- Time counted only when voice present matches VAD thresholds
- Timer pauses during silence automatically
- Session ends manually or on goal completion
- Edge noise (TV/music) minimally inflates counted seconds
Status: Ready

### US-002 (Epic: EP-02)
As a child, I want to see a token reward pop when I meet my goal so that I feel progress and motivation
Acceptance Criteria:

- Token reward triggers exactly once per goal session
- Token total increments immediately after reward
- Pop animation completes within 400ms
- Reward logged to persistent store
Status: Ready

### US-003 (Epic: EP-03)
As a parent/child, I want to switch between EN and ES passages so that both languages are supported at home
Acceptance Criteria:

- EN and ES passage lists load from JSON manifest
- Selecting locale filters list without restart
- Missing locale file fails gracefully (no crash)
- UI strings change with locale selection
Status: Ready

### US-004 (Epic: EP-04)
As a parent, I want strict mode to require both speaking and caret advancement so that attention remains focused
Acceptance Criteria:

- Strict mode counts time only with voice + periodic caret movement
- Relaxed mode unaffected by caret inactivity
- Cadence rule modifiable via settings.json
- Disabling strict mid-session stops caret requirement
Status: Queued

## Tasks
### TK-0000 US-000 design doc (Story: US-000, Type: Design, Est: XS)
Outcome: Design doc describing splash screen implementation and platform build steps.
GDD Trace: GDDv1.1.md#platforms--tech
Dependencies: None
Acceptance Criteria:
- File `Documents/design/US-000-design.md` created
- Sections: Purpose, Platforms, Build Steps, Risks, Decisions
- At least 1 risk + 1 open question captured

### TK-0001 Splash screen prefab created (Story: US-000, Type: CreateAsset, Est: XS)
Outcome: Unity prefab for splash screen visual.
GDD Trace: GDDv1.1.md#uxui-lean
Dependencies: TK-0000
Acceptance Criteria:
- File `Assets/Prefabs/SplashScreen.prefab` exists
- Visual matches kid-friendly theme
- No console errors on instantiation

### TK-0002 Platform build validation (Story: US-000, Type: Test, Est: XS)
Outcome: App builds and launches with splash screen on Android, iPad, Windows.
GDD Trace: GDDv1.1.md#platforms--tech
Dependencies: TK-0000, TK-0001
Acceptance Criteria:
- Build instructions documented for each platform
- Splash screen visible for at least 1 second
- No startup errors or crashes
- Screenshots captured for each platform

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

### TK-0006 US-002 design doc (Story: US-002, Type: Design, Est: XS)
Outcome: Design clarifying reward trigger timing and animation handshake
GDD Trace: GDDv1.1.md#rewards
Dependencies: None
Acceptance Criteria:

- File `Documents/design/US-002-design.md` created
- Defines trigger event source and UI update order
- Identifies potential double-award race and mitigation

### TK-0007 Reward token manager persists totals (Story: US-002, Type: Code, Est: S)
Outcome: `TokenManager` loads/saves totals and provides atomic award method
GDD Trace: GDDv1.1.md#rewards
Dependencies: TK-0006
Acceptance Criteria:

- File `Assets/Scripts/Rewards/TokenManager.cs` created
- Method `Award(int amount)` atomic and idempotent per session goal
- Reads initial total from `logs.json`
- Persists updated total immediately after award
- Handles missing file by creating default

### TK-0008 Token pop animation prefab created (Story: US-002, Type: CreateAsset, Est: S)
Outcome: Prefab `TokenPop.prefab` with scale+ease animation 200–400ms
GDD Trace: GDDv1.1.md#rewards
Dependencies: TK-0006
Acceptance Criteria:

- File `Assets/Prefabs/TokenPop.prefab` exists
- Animation duration within 0.2–0.4s window
- No console errors on instantiation
- Reusable component exposes `Play()` method
- Style matches kid-friendly theme (color + bounce)

### TK-0009 Reward sequence triggers on goal (Story: US-002, Type: Code, Est: S)
Outcome: Session goal reach triggers token award then animation without race conditions
GDD Trace: GDDv1.1.md#rewards
Dependencies: TK-0004, TK-0007, TK-0008
Acceptance Criteria:

- Single animation instance per goal reached
- Token total UI reflects new value before animation end
- Award skipped if already granted this session
- Logs award event with timestamp to `logs.json`
- No blocking main thread >16ms during trigger

### TK-0010 US-003 design doc (Story: US-003, Type: Design, Est: XS)
Outcome: Design for content and localization loading pipeline
GDD Trace: GDDv1.1.md#content--localization-final
Dependencies: None
Acceptance Criteria:

- File `Documents/design/US-003-design.md` created
- Describes manifest parsing and locale switching strategy
- Notes fallback behavior on missing locale file

### TK-0011 Manifest loader parses passage groups (Story: US-003, Type: Code, Est: S)
Outcome: `ManifestLoader` loads JSON manifest into in-memory model
GDD Trace: GDDv1.1.md#content--localization-final
Dependencies: TK-0010
Acceptance Criteria:

- File `Assets/Scripts/Content/ManifestLoader.cs` created
- Public method `Load()` returns structured model (locales, groups, files)
- Graceful handling of missing file returns empty model
- No per-file allocations after load phase completes
- Unit test with sample manifest passes

### TK-0012 Locale switcher updates UI strings (Story: US-003, Type: Code, Est: S)
Outcome: Locale switching reloads UI text assets
GDD Trace: GDDv1.1.md#content--localization-final
Dependencies: TK-0010
Acceptance Criteria:

- File `Assets/Scripts/Localization/LocaleSwitcher.cs` created
- Method `SetLocale(code)` reloads UI strings and raises event
- Falls back to `en` if requested locale missing
- Locale change persists to settings on save
- No duplicate event firing for same locale

### TK-0013 Passage list generator filters by locale (Story: US-003, Type: Code, Est: S)
Outcome: UI passage list refreshed based on selected locale and difficulty
GDD Trace: GDDv1.1.md#content--localization-final
Dependencies: TK-0011, TK-0012
Acceptance Criteria:

- File `Assets/Scripts/Content/PassageListBuilder.cs` created
- Filters by locale + optional difficulty toggle
- Handles empty lists without error
- UI update under 50ms for sample content
- No GC allocations on refresh after first build

### TK-0014 US-004 design doc (Story: US-004, Type: Design, Est: S)
Outcome: Design defines strict mode caret cadence integration with VAD
GDD Trace: GDDv1.1.md#caretstrictness
Dependencies: None
Acceptance Criteria:

- File `Documents/design/US-004-design.md` created
- Defines interface for caret advancement events
- Specifies cadence parameterization (words per seconds window)
- Identifies failure modes (no caret, rapid spam) and handling

### TK-0015 Strict mode timer gate (Story: US-004, Type: Code, Est: M)
Outcome: Timer counts only when both voice active and cadence satisfied
GDD Trace: GDDv1.1.md#caretstrictness
Dependencies: TK-0003, TK-0014
Acceptance Criteria:

- Modification or extension class integrates caret events with existing timer
- Cadence threshold adjustable via settings key `strictCaretCadence`
- Fallback path leaves relaxed mode unchanged
- Unit test simulates cadence pass/fail scenario
- No added allocations during 5 minute simulated session

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

### TK-0017 Logs writer appends session data (Story: US-002, Type: Code, Est: S)
Outcome: `LogWriter` appends session summary safely to logs.json
GDD Trace: GDDv1.1.md#settings--logs-json-locked
Dependencies: TK-0007
Acceptance Criteria:

- File `Assets/Scripts/Logging/LogWriter.cs` created
- Appends session object with timestamp, passageId, secondsCounted, goalMet
- Rolling in-memory cache prevents duplicate write on fast repeat
- Handles file growth without re-reading entire file each append
- Unit test verifies append sequence order

### TK-0018 Minimal reading view UI layout (Story: US-001, Type: UX, Est: S)
Outcome: Basic Reading View displaying passage text, timer, voice indicator, end session button
GDD Trace: GDDv1.1.md#uxui-lean
Dependencies: TK-0004, TK-0013
Acceptance Criteria:

- Prefab `ReadingView.prefab` exists
- Timer updates visually at least 2Hz
- Voice indicator toggles within 150ms of state change
- End session button triggers controller stop
- Layout scales legibly on tablet and small laptop

### TK-0019 Reward screen UI shows token increment (Story: US-002, Type: UX, Est: S)
Outcome: Reward screen reflects new total with animation integration point
GDD Trace: GDDv1.1.md#uxui-lean
Dependencies: TK-0009
Acceptance Criteria:

- Prefab `RewardScreen.prefab` exists
- Displays token total updated from TokenManager
- Provides Back to Home button functioning
- Uses token pop animation component hook
- Layout passes accessibility color contrast (basic manual check)

### TK-0020 Locale toggle control on home (Story: US-003, Type: UX, Est: S)
Outcome: Home screen locale toggle updates passage list + UI strings
GDD Trace: GDDv1.1.md#uxui-lean
Dependencies: TK-0012, TK-0013
Acceptance Criteria:

- Control present on home screen UI prefab
- Changing value triggers SetLocale and list refresh
- Disabled state when only one locale present
- Visual state persists across app restart
- No layout shift >10px on toggle

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
- Branch pattern: feat/<task-id>-<slug>
- Labels suggestion: type:<Type>, size:<XS|S|M>
- Status field used in file maps to board columns

## Flow Metrics Hints
- Cycle Time: Start = first status change to In Progress, End = Done
- Throughput: Done tasks per calendar week (exclude design optionally)
- Aging WIP: Highlight tasks >3 days In Progress
