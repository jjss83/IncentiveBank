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

- US-001: [Unity Config] Multi-Platform Build Configuration
- US-002: [UI Development] Bootstrap Scene with Splash Display  
- US-003: [Platform Setup] Mobile Permission Handling
- US-004: [Audio Systems] Microphone Device Detection
- US-005: [Script Dev] Platform Validation Logic
- US-006: [Asset Creation] Build Configuration Assets
- US-007: [Testing] Platform Compatibility Test Suite
- US-008: [CI/CD] Automated Build Pipeline

### US-001: [Unity Config] Multi-Platform Build Configuration

As a developer, I want Unity configured for Android, iOS, and Windows builds so that I can deploy the same codebase across all target platforms.

#### Acceptance Criteria

- Unity project builds successfully for Android API 23+ without errors
- Unity project builds successfully for iOS 12+ without errors  
- Unity project builds successfully for Windows 10+ without errors
- Build settings are configured for appropriate architecture targets (ARM64, x64)
- Platform-specific player settings are properly configured

#### Dependencies

- None

#### Notes

- References GDD Executive Summary platform requirements
- Aligns with Unity Dev Basics for project configuration
- Supports GDD risk mitigation strategy for Unity beta/LTS fallback

### US-002: [UI Development] Bootstrap Scene with Splash Display

As a user, I want to see a splash screen when the app launches so that I know the app is loading properly on my device.

#### Acceptance Criteria

- Bootstrap scene displays splash image on app launch
- Splash screen is visible for minimum 2 seconds
- Splash image scales appropriately across different screen resolutions
- Scene transitions smoothly after splash display
- UI elements are properly configured with TextMeshPro

#### Dependencies

- US-001: Multi-platform build configuration must be complete

#### Notes

- References GDD minimal app creation requirement
- Follows Unity Dev Basics for scene structure and UI setup
- Uses StreamingAssets for platform-agnostic splash image

### US-003: [Platform Setup] Mobile Permission Handling

As a mobile user, I want microphone permissions properly requested so that the app can access audio input for voice detection.

#### Acceptance Criteria

- Android manifest includes RECORD_AUDIO permission
- iOS Info.plist includes microphone usage description
- Permission requests are triggered on app first launch
- App handles permission denial gracefully with user feedback
- Permission status can be checked programmatically

#### Dependencies

- US-001: Multi-platform build configuration must be complete

#### Notes

- References GDD Acceptance Criteria #3 for microphone permissions
- Aligns with platform-specific best practices from risk mitigation
- Critical for core VAD functionality in future epics

### US-004: [Audio Systems] Microphone Device Detection

As a developer, I want to detect available microphone devices so that I can verify audio input capability across platforms.

#### Acceptance Criteria

- Unity Microphone API successfully enumerates available devices
- Device detection works on Android tablets and phones
- Device detection works on iPad devices
- Device detection works on Windows laptops with default WASAPI
- Empty device list is handled gracefully with user feedback

#### Dependencies

- US-003: Mobile permission handling must be complete

#### Notes

- References GDD Audio/Perf requirements for Windows WASAPI settings
- Supports GDD Acceptance Criteria #4 for Microphone.devices functionality
- Foundation for future VAD implementation

### US-005: [Script Dev] Platform Validation Logic

As a developer, I want platform-specific validation logic so that I can verify core functionality works correctly on each target platform.

#### Acceptance Criteria

- PlatformValidator component checks device capabilities on startup
- Validation results are logged for debugging purposes
- Platform-specific quirks are detected and handled appropriately
- Validation failures are reported to user with actionable feedback
- Component follows composition over inheritance principles

#### Dependencies

- US-004: Microphone device detection must be complete

#### Notes

- Implements Unity Dev Basics architectural patterns
- Supports GDD risk mitigation for audio API limitations
- Enables graceful fallbacks per GDD audio guidelines

### US-006: [Asset Creation] Build Configuration Assets

As a developer, I want ScriptableObject assets for build settings so that I can manage platform-specific configurations without code changes.

#### Acceptance Criteria

- BuildConfig ScriptableObject stores platform-specific settings
- Configuration assets are created for Android, iOS, and Windows
- Settings include target API levels, architectures, and permissions
- Assets are version-controlled and shareable across team
- Configuration can be swapped without recompilation

#### Dependencies

- US-001: Multi-platform build configuration must be complete

#### Notes

- Follows Unity Dev Basics preference for ScriptableObjects for configuration
- Supports maintainable platform-specific settings management
- Enables flexible deployment configuration

### US-007: [Testing] Platform Compatibility Test Suite

As a developer, I want automated tests for platform compatibility so that I can verify functionality works correctly across all target platforms.

#### Acceptance Criteria

- Edit-mode tests validate build configuration logic
- Play-mode tests verify scene loading and component initialization
- Tests cover microphone device detection on each platform
- Test suite runs in CI/CD pipeline without failures
- Tests provide clear failure messages for debugging

#### Dependencies

- US-005: Platform validation logic must be complete

#### Notes

- Implements Testing in Unity guide for edit-mode and play-mode coverage
- Supports CI/CD integration for automated validation
- Critical for maintaining platform compatibility over time

### US-008: [CI/CD] Automated Build Pipeline

As a developer, I want automated builds for all platforms so that I can ensure consistent deployable artifacts without manual intervention.

#### Acceptance Criteria

- CI/CD pipeline builds Android APK/AAB artifacts
- CI/CD pipeline builds iOS IPA artifacts (when certificates available)
- CI/CD pipeline builds Windows executable artifacts
- Build artifacts are tagged with version information
- Pipeline runs tests before generating build artifacts

#### Dependencies

- US-007: Platform compatibility test suite must be complete

#### Notes

- References epic CI/CD section for build automation requirements
- Supports artifact management and version tagging
- Foundation for reliable deployment process
