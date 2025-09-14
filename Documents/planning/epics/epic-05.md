---
id: EP-05
name: Rewards, Token Pop & Logging
status: Queued
owner: TBD
source_gdd:
  - GDDv1.1.md#rewards
  - GDDv1.1.md#settings-logs-json-locked
created: 2025-09-14
unity:
  version: 6000.0.48f1
  targets:
    - Windows (Editor + build)
    - Android (if applicable)
    - iOS (if applicable)
---

# EP-05 Rewards, Token Pop & Logging

## User Stories (Index)

- To be defined via the Epic Unfold prompt

## Workflow Overview

On meeting the session goal, grant tokens, play a tiny token pop animation, and append aggregate stats to `logs.json`.

## Epic Narrative

Rewards provide a tangible sense of progress. This epic connects timing to tokens with a lightweight animation and logging.

## Business Value

- Motivates continued use via clear goals and rewards
- Provides basic analytics for iteration (local and private)

## Goals & Non-Goals

- Goals:
  - Detect goal crossing from the session timer
  - Increment token total and persist to `logs.json`
  - Play a 200–400ms token pop animation
- Non-Goals:
  - Streaks, complex incentives (post-MVP)

## Acceptance Criteria

- On goal reached, tokens increase by `rewardTokensPerGoal`
- Token pop animation plays once, finishes within 400ms
- `logs.json` appends a session entry with fields per GDD
- No audio recordings saved

## Technical Design

- Token service with in-memory total and persistence
- Animation: simple scale/ease on token icon
- Log writer appends sessions with timestamps and fields

### Architecture Sketch

- Scripts:
  - `Assets/Scripts/Rewards/TokenService.cs`
  - `Assets/Scripts/Rewards/TokenPop.cs`
  - `Assets/Scripts/Logging/SessionLogWriter.cs`

### CI/CD Notes

- Editor tests for log append and token math

## Risks & Mitigations

- Animation timing variability → use unscaled time and fixed duration windows

## Traceability

- GDD: `GDDv1.1.md#rewards`, `GDDv1.1.md#settings-logs-json-locked`
