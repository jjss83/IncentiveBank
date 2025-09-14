---
id: EP-04
name: Content & Localization Loader
status: Queued
owner: TBD
source_gdd: GDDv1.1.md#content-localization-final
created: 2025-09-14
unity:
  version: 6000.0.48f1
  targets:
    - Windows (Editor + build)
    - Android (if applicable)
    - iOS (if applicable)
---

# EP-04 Content & Localization Loader

## User Stories (Index)

- To be defined via the Epic Unfold prompt

## Workflow Overview

Load passage content from a fixed folder structure and drive UI strings from locale files (EN/ES).

## Epic Narrative

Externalizing content and UI strings allows fast iteration and bilingual support without app updates.

## Business Value

- Enables content updates and localization without code changes
- Matches GDD structure for `/Content` and `/Localization`

## Goals & Non-Goals

- Goals:
  - Parse `manifest.json` and list passages by group
  - Load passage JSONs for display
  - Load UI strings from `ui_en.json` and `ui_es.json`
- Non-Goals:
  - In-app editors (parent loop out of scope for MVP)

## Acceptance Criteria

- Manifest loads; groups and files displayed by locale and difficulty
- Switching locale updates UI strings and passage lists
- Handles missing optional fields (e.g., `wordBoundaries: null`) gracefully

## Technical Design

- JSON models for Manifest, Passage, and UI dictionaries
- Data path configurable via settings; default to `/Content` and `/Localization`
- Simple content loader with caching

### Architecture Sketch

- Scripts:
  - `Assets/Scripts/Content/ManifestLoader.cs`
  - `Assets/Scripts/Content/PassageLoader.cs`
  - `Assets/Scripts/Localization/UiStrings.cs`

### CI/CD Notes

- Editor tests for JSON parsing and path resolution

## Risks & Mitigations

- Path/platform differences → normalize paths and test across targets

## Traceability

- GDD: `GDDv1.1.md#content-localization-final`
