---
id: EP-03
name: Strict Mode & Caret Cadence
status: Queued
owner: TBD
source_gdd: GDDv1.1.md#caretstrictness
created: 2025-09-14
unity:
  version: 6000.0.48f1
  targets:
    - Windows (Editor + build)
    - Android (if applicable)
    - iOS (if applicable)
---

# EP-03 Strict Mode & Caret Cadence

## User Stories (Index)

- To be defined via the Epic Unfold prompt

## Workflow Overview

Gate timer progression on both VoiceActive and a minimum caret advancement cadence; provide tunable thresholds.

## Epic Narrative

Strict mode encourages active engagement by requiring visible progress through the text alongside speaking.

## Business Value

- Increases reading fidelity for certain content categories
- Optional mode matches GDD strictness requirement

## Goals & Non-Goals

- Goals:
  - Caret advancement cadence (e.g., ≥1 word / 3s; soften per GDD risks)
  - Timer counts only when both VoiceActive and caret cadence satisfied
  - Clear UI indication when strict condition is unmet
- Non-Goals:
  - Phoneme-level ASR

## Acceptance Criteria

- With strict mode on, timer increments only when both conditions are true
- When cadence is below threshold, a subtle UI nudge appears; timer pauses
- Thresholds configurable; defaults align with GDD

## Technical Design

- Caret controller to advance indices across words or spans
- Cadence tracker (rolling window)
- Strict gate combining VAD and cadence

### Architecture Sketch

- Scripts:
  - `Assets/Scripts/Reading/CaretController.cs`
  - `Assets/Scripts/Reading/CadenceTracker.cs`
  - `Assets/Scripts/Reading/StrictGate.cs`

### CI/CD Notes

- Playmode tests for gate logic at boundary conditions

## Risks & Mitigations

- User fatigue in strict mode → soften cadence default (e.g., 1 word/5s) and provide visual aid

## Traceability

- GDD: `GDDv1.1.md#caretstrictness`
