---
id: EP-02
name: Reading Session Timer & Flow
status: Queued
owner: TBD
source_gdd: GDDv1.1.md#acceptance-criteria-updated
created: 2025-09-14
unity:
  version: 6000.0.48f1
  targets:
    - Windows (Editor + build)
    - Android (if applicable)
    - iOS (if applicable)
---

# EP-02 Reading Session Timer & Flow

## User Stories (Index)

- To be defined via the Epic Unfold prompt

## Workflow Overview

Count session time only when VoiceActive is true; pause during silence; expose a timer chip in the Reading view.

## Epic Narrative

Accurate session timing drives rewards and player motivation. This epic wires VAD into a stable session timer and simple UI.

## Business Value

- Aligns with MVP acceptance criteria for counting only during voice
- Establishes the core reading loop foundation

## Goals & Non-Goals

- Goals:
  - Session timer increments only while VoiceActive
  - Pause during silence; resume seamlessly
  - Timer chip visible and legible in the Reading view
- Non-Goals:
  - Strict mode gating (EP-03)
  - Rewards (EP-05)

## Acceptance Criteria

- Timer increases only when VoiceActive; holds steady during silence
- No cumulative drift beyond 0.1s over 5 minutes of simulated input
- UI timer chip updates at a reasonable cadence without GC allocs

## Technical Design

- `SessionTimer` subscribes to VAD events or polls VoiceActive
- Update loop avoids per-frame allocations; uses pooled StringBuilder for UI
- Expose elapsed seconds and ms for testing

### Architecture Sketch

- Scripts:
  - `Assets/Scripts/Session/SessionTimer.cs`
  - `Assets/Scripts/UI/TimerChip.cs`

### CI/CD Notes

- Playmode test for drift and alloc checks

## Risks & Mitigations

- Timer jitter on low-end devices → decouple tick from frame rate; accumulate using Time.unscaledDeltaTime

## Traceability

- GDD: `GDDv1.1.md#acceptance-criteria-updated`
