---
generated_at: 2025-09-11
source_gdd: GDDv1.1.md
workflow: kanban
---

# Epics

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