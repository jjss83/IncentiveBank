# Pipeline Validation & Device Bring-up

## Workflow

1. **Environment Setup**: Configure Unity project with target platform builds (Android, iOS, Windows)
2. **Minimal App Creation**: Build simplest possible app with splash screen to validate core pipeline
3. **Platform Deployment**: Deploy to each target device and verify successful launch
4. **CI/CD Integration**: Ensure automated builds work across all platforms
5. **Device Compatibility**: Validate microphone permissions and basic audio access on real hardware

## Narrative

A minimal app runs on all target devices and displays a splash image to validate build, signing, and deployment pipelines. This epic serves as the foundation that de-risks the entire project by proving end-to-end compatibility before implementing complex features like voice detection and reading mechanics.

## Value

De-risks the project by proving end-to-end CI/CD and device compatibility early, enabling faster feedback on real hardware. Establishes confidence in the development pipeline and reduces technical debt by catching platform-specific issues before feature development begins.

## Goals

- Validate Unity build pipeline works for Android, iOS (iPad), and Windows platforms
- Confirm app deployment and launch on target devices  
- Establish microphone permission handling per platform
- Verify basic audio input access through Unity Microphone API
- Set up automated CI/CD pipeline for consistent builds
- Create minimal project structure following Unity dev basics (composition over inheritance, ScriptableObjects for config)

## Acceptance Criteria

- App builds successfully for Android, iOS, and Windows without errors
- App launches on each target platform and displays splash screen
- Microphone permissions are properly requested and granted on mobile platforms
- `Microphone.devices` returns available devices on each platform
- Basic audio input can be accessed through Unity Microphone API
- CI/CD pipeline produces consistent builds across all target platforms
- Project structure follows established Unity development patterns from dev guides

## Technical Design

- **Unity Version**: Latest beta (with fallback to latest LTS per GDD risk mitigation)
- **Build Targets**: Android API level 23+, iOS 12+, Windows 10+
- **Architecture**: Single scene with minimal bootstrap MonoBehaviour
- **Audio**: Unity Microphone API with platform-specific device enumeration
- **Permissions**: Android manifest permissions, iOS info.plist microphone usage description
- **Config**: ScriptableObject for build settings and platform-specific configurations

## Architecture

```
Assets/
├── Scenes/
│   └── Bootstrap.unity (minimal scene with splash)
├── Scripts/
│   ├── Bootstrap/
│   │   ├── AppBootstrap.cs (main entry point)
│   │   └── PlatformValidator.cs (device/audio validation)
│   └── Config/
│       └── BuildConfig.cs (ScriptableObject for settings)
├── StreamingAssets/
│   └── splash.png (platform-agnostic splash image)
└── Tests/
    ├── EditMode/
    │   └── PlatformValidatorTests.cs
    └── PlayMode/
        └── BootstrapIntegrationTests.cs
```

## CI/CD

- **Build Automation**: GitHub Actions or Unity Cloud Build for multi-platform builds
- **Testing**: Automated edit-mode and play-mode test execution
- **Artifact Management**: Store builds for each platform with version tagging
- **Device Testing**: Manual validation on representative devices (Android tablet, iPad, Windows laptop)
- **Performance Baseline**: Establish baseline metrics for app launch time and memory usage

## Risks

- **Unity Beta Instability**: Latest Unity beta may have platform-specific bugs
  - *Mitigation*: Maintain parallel setup with latest LTS version for quick fallback
- **Platform Permission Changes**: Mobile platform permission requirements may change
  - *Mitigation*: Follow platform-specific best practices and test on multiple OS versions
- **Audio API Limitations**: Unity Microphone API may have device-specific quirks
  - *Mitigation*: Test on diverse hardware; implement graceful fallbacks per GDD audio guidelines
- **CI/CD Platform Compatibility**: Build automation may not support all target platforms equally
  - *Mitigation*: Start with local builds, incrementally add CI/CD per platform

## Traceability

- **GDD References**: 
  - Executive Summary: "Android tablet/phone, iPad, Windows laptop with mic • Unity (latest beta; fallback to latest LTS if needed)"
  - Audio/Perf: "Windows: keep default WASAPI settings"
  - Acceptance Criteria #6: "Works on Android, iPad, and Windows with default mic settings"
- **Dev Guide Alignment**:
  - Unity Dev Basics: Bootstrap scene pattern, ScriptableObject configuration, platform checks
  - Testing in Unity: Edit-mode tests for logic, play-mode tests for integration
  - Feature Strategy Template: Risk mitigation for Unity beta stability

## User Stories
