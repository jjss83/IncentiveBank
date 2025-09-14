---
id: EP-01
name: Audio Input & VAD Core
status: Queued
owner: TBD
source_gdd: GDDv1.1.md#vad-voice-activity-detection
created: 2025-09-14
unity:
  version: 6000.0.48f1
  targets:
    - Windows (Editor + build)
    - Android (if applicable)
    - iOS (if applicable)
---

# EP-01 Audio Input & VAD Core

## User Stories (Index)

- To be defined via the Epic Unfold prompt

## Workflow Overview

Initialize microphone input, calibrate ambient noise, compute RMS per frame, and expose a stable VoiceActive flag using hysteresis and debounce.

## Epic Narrative

Reliable voice activity detection underpins the reading loop. This epic establishes a zero-allocation, low-CPU audio path with robust activation/deactivation behavior.

## Business Value

- Enables accurate session timing and strict mode gating
- Provides consistent behavior across devices by calibrating to ambient noise

## Goals & Non-Goals

- Goals:
  - Ambient floor calibration on start
  - RMS energy per 10–30ms frame
  - Hysteresis thresholds with 600ms debounce
  - Optional spectral flatness filter to reduce TV/music false positives
  - Zero allocations per audio frame; <3% CPU on mid devices
- Non-Goals:
  - UI/UX polish beyond basic indicators
  - Speech recognition/phonemes (post-MVP)

## Acceptance Criteria

- Calibrates ambient floor at startup; exposes calibration time in logs
- VoiceActive only true when RMS exceeds enter threshold and remains until exit threshold
- 600ms debounce prevents blips during borderline input
- Optional spectral flatness reduces music/TV false positives in a test clip
- Zero allocations detected across 5k audio frames; CPU <3% on a mid device

## Technical Design

- Unity Microphone API for input; circular buffer for frames
- Frame windowing at 10–30ms; RMS computation
- Hysteresis enter/exit thresholds + debounce timer
- Optional spectral flatness metric per frame
- Threading: ensure main-thread safe signaling for UI

### Architecture Sketch

- Scripts:
  - `Assets/Scripts/Audio/VadInput.cs`
  - `Assets/Scripts/Audio/VadProcessor.cs`
- Data:
  - `VadConfig` with thresholds/debounce; inspector-exposed

### CI/CD Notes

- Editor tests for RMS/hysteresis behavior
- Performance micro-benchmarks (playmode) for allocs and CPU

## Risks & Mitigations

- Device mic variance → per-device calibration step with sane defaults
- GC spikes → preallocate buffers and reuse

## Traceability

- GDD: `GDDv1.1.md#vad-voice-activity-detection`
