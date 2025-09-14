---
id: EP-00
name: Pipeline Validation & Device Bring-up
status: Active
owner: TBD
source_gdd: GDDv1.1.md#pipeline-validation
created: 2025-09-13
unity:
  version: 6000.0.48f1
  targets:
    - Windows (Editor + build)
    - Android (if applicable)
    - iOS (if applicable)
    - WebGL (if applicable)
---

# EP-00 Pipeline Validation & Device Bring-up

## Workflow Overview

This epic establishes the foundation to build, run, and validate the app across target devices. The expected workflow:

1. Create minimal scenes (Bootstrap → Splash) and placeholder assets
2. Add bootstrap scripts and version label; confirm Editor Play works
3. Configure Unity Player and Build Settings for target platforms
4. Configure identifiers, versioning, and splash screen
5. Produce a local platform build; smoke test on device/emulator
6. Introduce minimal CI build job to produce artifacts
7. Document signing placeholders for mobile platforms (if targeted)

## Epic Narrative

A minimal application builds and runs on all target devices and displays a splash image. This validates the full build, signing, deployment, and runtime pipeline, ensuring we can iterate on real hardware quickly.

## Business Value

- De-risks the project early by proving CI/CD and platform compatibility
- Unblocks downstream epics by ensuring stable assets and player settings
- Enables faster feedback loops and early performance baselines

## Goals & Non-Goals

- Goals:
  - Project builds green locally and in CI for target platforms
  - App launches to a splash image without errors or warnings spam
  - Basic telemetry/logging confirms startup path executed
  - Signing/packaging verified (where applicable)
- Non-Goals:
  - Feature content (reading, rewards, localization) — covered by other epics
  - UI polish beyond a minimal splash

## Acceptance Criteria

- Build succeeds for each declared target platform
- App starts and displays splash image within 3s on mid-tier devices
- No blocking errors in logs; warnings < 3 on clean start
- Artifact produced by CI and is installable/runnable
- Basic startup log line present, including app version and platform

## Technical Design

- Unity Player Settings minimal configuration for target platforms
- Scene Bootstrap: `Assets/Scenes/Bootstrap.unity` loads `Splash.unity`
- Splash Scene: displays static image asset and version label (bottom-right)
- Logging: simple startup log written to a platform-appropriate sink
- CI outline: pipeline jobs for Editor validation + per-platform build

### Architecture Sketch

- Scenes:
  - `Assets/Scenes/Bootstrap.unity`: sets screen orientation and loads Splash
  - `Assets/Scenes/Splash.unity`: displays `Assets/Art/Textures/splash.png` and version text
- Scripts:
  - `Assets/Scripts/Bootstrap/Bootstrap.cs`: runtime guardrails, logs, and scene handoff
  - `Assets/Scripts/Bootstrap/VersionLabel.cs`: renders app version string from `Application.version`
- Assets:
  - `Assets/Art/Textures/splash.png` (placeholder)
- Config:
  - `ProjectSettings/PlayerSettings` minimal adjustments by platform

### CI/CD Notes

- Use Unity Build (e.g., UBA/Builder or Unity CLI) to create artifacts per platform
- Cache Library to speed builds; ensure deterministic version via `ProjectVersion.txt`
- Outputs:
  - Windows: Player build folder (zip)
  - Android: `.aab` or `.apk`
  - iOS: Xcode project (archive step outside CI, optional)
  - WebGL: Build folder

## Risks & Mitigations

- Platform signing complexity → Document keystore/provisioning and automate where possible
- Device-specific resource constraints → Keep splash lightweight; profile startup
- CI runner capabilities → Use matrix builds with appropriate images

## Traceability

- GDD: `GDDv1.1.md#pipeline-validation` (add this section if missing)
- Backlog: `Documents/planning/kanban-backlog.md#ep-00-pipeline-validation--device-bring-up`

---

## User Stories

### US-000 (Epic: EP-00) Build & launch shows splash

As a developer, I need builds to install and launch showing a splash so we can validate the pipeline early.

#### Story Acceptance Criteria

- Windows Editor Play Mode enters Splash scene without errors
- Windows standalone build launches and shows splash
- Android/iOS/WebGL builds (where targeted) launch and show splash
- A startup log line is written: `Startup: version=<v> platform=<p>`
- CI job produces artifacts for at least one non-editor platform

## Tasks

### TK-0000 Create bootstrap and splash scenes (Type: CreateAsset, Est: XS)

Outcome: `Assets/Scenes/Bootstrap.unity` and `Assets/Scenes/Splash.unity` exist, splash shows placeholder image and version label
Dependencies: None
Acceptance Criteria:

- Scenes created and added to build settings (Bootstrap first)
- Placeholder `splash.png` imported (no warnings)
- Version label visible and reads `Application.version`

### TK-0001 Add bootstrap scripts (Type: Code, Est: XS)

Outcome: `Bootstrap.cs` loads Splash scene and logs startup info
Dependencies: TK-0000
Acceptance Criteria:

- File `Assets/Scripts/Bootstrap/Bootstrap.cs` with scene load and log
- Log format: `Startup: version=<v> platform=<p>`
- No GC alloc spikes during load

### TK-0002 Add version label script (Type: Code, Est: XS)

Outcome: `VersionLabel.cs` displays version and target platform
Dependencies: TK-0000
Acceptance Criteria:

- File `Assets/Scripts/Bootstrap/VersionLabel.cs` reads `Application.version`
- Displays platform via `Application.platform`
- No console errors in editor or build

### TK-0003 Minimal CI build job (Type: Ops, Est: S)

Outcome: Automated build produces installable artifact for at least one platform
Dependencies: TK-0000
Acceptance Criteria:

- CI config checked in (`.github/workflows/unity-build.yml` or similar)
- Build completes using `ProjectSettings/ProjectVersion.txt`
- Artifact uploaded on successful run

### TK-0004 Platform signing placeholders (Type: Ops, Est: XS)

Outcome: Document signing steps and placeholder secrets for Android/iOS (if targeted)
Dependencies: TK-0003
Acceptance Criteria:

- `Documents/build/signing-notes.md` created
- Placeholder secrets names listed (without real keys)
- Notes on how to request/provision real credentials

### TK-0005 Unity Player Settings baseline (Type: Config, Est: XS)

Outcome: Minimal player settings applied for each targeted platform
Dependencies: TK-0000
Step-by-step:

1. Open `Edit > Project Settings > Player`
2. Under Resolution/Presentation, disable unnecessary features for splash-only
3. Under Other Settings, ensure `Scripting Backend` is set to IL2CPP (for mobile), API compatibility `.NET Standard 2.1`
4. Set `Allow downloads over HTTP` to `Not allowed` (default secure)
5. Apply platform-specific defaults (e.g., Android: `ARM64` only)
Acceptance Criteria:

- Settings saved to `ProjectSettings/ProjectSettings.asset`
- No new warnings on entering Play Mode
- Platform tabs show expected backend/architecture selections

### TK-0006 Build Settings configuration (Type: Config, Est: XS)

Outcome: Scenes added to Build Settings with correct order
Dependencies: TK-0000
Step-by-step:

1. Open `File > Build Settings`
2. Add `Assets/Scenes/Bootstrap.unity` (top) and `Assets/Scenes/Splash.unity`
3. Select a target platform (e.g., Windows, WebGL); click `Switch Platform`
4. Verify scripting symbols are empty (for minimal build)
Acceptance Criteria:

- Build Settings includes both scenes in correct order
- Target platform switch completes without errors
- Test build completes locally without extra configuration

### TK-0007 Platform module verification (Type: Config, Est: XS)

Outcome: Required Unity modules installed for selected targets
Dependencies: None
Step-by-step:

1. In Unity Hub, verify modules for chosen targets are installed (e.g., Android, iOS, WebGL)
2. If missing, install modules matching Editor `6000.0.48f1`
3. Reopen project and confirm platform switch works
Acceptance Criteria:

- Module presence verified in Unity Hub
- Platform switch and local build do not error due to missing modules

### TK-0008 Identifiers and versioning (Type: Config, Est: XS)

Outcome: App identifiers and version string set consistently across platforms
Dependencies: TK-0005
Step-by-step:

1. Open `Edit > Project Settings > Player`
2. Set `Version` to `0.0.1`
3. Android: Set `Package Name` (e.g., `com.jjss83.incentivebank`)
4. iOS: Set `Bundle Identifier` similarly; set minimum iOS version per target policy
5. Windows/WebGL: leave identifiers default; ensure version applied
Acceptance Criteria:

- `Application.version` reads `0.0.1` at runtime
- Android/iOS identifiers are valid and unique

### TK-0009 Splash screen configuration (Type: Config, Est: XS)

Outcome: Unity splash (or custom) shows within 3s with placeholder image
Dependencies: TK-0000
Step-by-step:

1. Open `Edit > Project Settings > Player > Splash Image`
2. Use Unity splash or add `Assets/Art/Textures/splash.png` as a custom image
3. Set background color and scaling to fit most devices
4. Ensure total splash duration is minimal (target < 2s if using Unity splash)
Acceptance Criteria:

- Splash image displays correctly in Editor Play and local builds
- No layout/stretch artifacts on common resolutions
