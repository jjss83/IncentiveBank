---
id: EP-06
name: Basic Session Controls (Start/Stop) & Flow Validation
status: Active
owner: TBD
source_gdd:
  - GDDv1.1.md#core-loops-compact
  - GDDv1.1.md#uxui-lean
  - GDDv1.1.md#roadmap-unchanged-structure
created: 2025-09-14
unity:
  version: 6000.0.48f1
  targets:
    - Windows (Editor + build)
    - Android (if applicable)
    - iOS (if applicable)
    - WebGL (if applicable)
---

# EP-06 Basic Session Controls (Start/Stop) & Flow Validation

## User Stories (Index)

- To be defined via the Epic Unfold prompt

## Workflow Overview

This epic introduces a minimal Start/Stop session control to validate the end-to-end flow (navigation, state, logging) without audio/VAD. The expected workflow:

1. From Home, navigate to Reading View
2. Tap Start to enter an active session state (UI reflects Active)
3. Tap Stop to end session and return to Home (or a simple summary)
4. Write basic logs for start/stop events
5. Ensure build and runtime are error-free across targets

## Epic Narrative

A simple Start/Stop interaction validates our navigation and state-handling path early. By deferring audio and timing, we can harden scene flow, UI wiring, and logs while keeping the scope extremely small.

## Business Value

- De-risks early by proving the session flow and navigation
- Enables faster iteration before audio/VAD integration
- Keeps UI and logging contracts stable for downstream epics

## Goals & Non-Goals

- Goals:
  - Minimal UI with Start and Stop controls
  - Clear state transitions and navigation between Home and Reading
  - Basic logging for start/stop events
  - Zero errors, warnings ≤ 2 on clean start/stop
- Non-Goals:
  - Audio capture, VAD, or timer counting (covered by EP-01/EP-02)
  - Rewards/token animation (covered by EP-05)

## Acceptance Criteria

- Home → Reading navigation works; Back path returns as designed
- Start sets session state Active; UI reflects Active state immediately
- Stop ends session and returns to Home (or shows minimal summary)
- Logs contain start and stop entries with timestamps
- No errors; warnings ≤ 2 on clean start/stop in Editor and build

## Technical Design

- Scenes: `Home.unity` and `Reading.unity` (or a single scene with simple view switching)
- UI: TMP-based buttons for Start and Stop; state indicator chip
- State: `SessionController` singleton or component handling Active flag
- Logging: `Debug.Log` lines for start/stop with UTC timestamps
- Navigation: `SceneManager` load or UI panel toggles for simplicity

### Architecture Sketch

- Scenes:
  - `Assets/Scenes/Home.unity`: Start Reading button → Reading
  - `Assets/Scenes/Reading.unity`: Start and Stop buttons, state indicator
- Scripts:
  - `Assets/Scripts/Session/SessionController.cs`: holds state, exposes Start/Stop
  - `Assets/Scripts/UI/Navigation.cs`: helper for scene/view transitions
- Assets:
  - Placeholder buttons and layout
- Config:
  - Add scenes to Build Settings in proper order

### CI/CD Notes

- Build a lightweight Windows/WebGL artifact to verify flow
- Cache Library to speed builds; ensure deterministic `ProjectVersion.txt`

## Risks & Mitigations

- Over-engineering early → Keep to minimal scenes and scripts, no premature abstraction
- Navigation complexity → Prefer single-scene view toggles if simpler

## Traceability

- GDD: `GDDv1.1.md#core-loops-compact`, `GDDv1.1.md#uxui-lean`, `GDDv1.1.md#roadmap-unchanged-structure`
- Backlog: add when kanban is updated
