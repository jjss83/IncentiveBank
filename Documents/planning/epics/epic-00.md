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

#### Tasks

- Design Unity Multi-Platform Architecture (Design, S)
  - Define build target configurations for Android, iOS, Windows
  - Specify architecture requirements (ARM64, x64)
  - Document platform-specific player settings
  - Create architecture decision records
  - Validate against GDD platform requirements
- Configure Android Build Settings (Config, S)
  - Set target API level to 23+
  - Configure ARM64 architecture
  - Set appropriate texture compression
  - Configure signing and security settings
  - Validate build output structure
- Configure iOS Build Settings (Config, S)
  - Set minimum iOS version to 12+
  - Configure ARM64 architecture
  - Set appropriate texture compression for Metal
  - Configure bundle identifier and signing
  - Validate build output structure
- Configure Windows Build Settings (Config, M)
  - Set minimum Windows version to 10+
  - Configure x64 architecture
  - Set appropriate graphics API (DirectX/OpenGL)
  - Configure executable and data folder structure
  - Test on representative Windows hardware
  - Validate WASAPI audio settings per GDD
- [Edit-mode Test] Platform Build Configuration Tests (Test, S)
  - Verify build settings are correctly applied per platform
  - Test architecture configuration validation
  - Validate platform-specific player settings
  - Test build target switching functionality
  - Ensure tests run in CI/CD pipeline

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

#### Tasks

- Design Bootstrap Scene Layout (Design, S)
  - Define scene hierarchy and component structure
  - Plan splash image display timing and transitions
  - Design scalable UI layout for multiple resolutions
  - Document scene loading and initialization flow
  - Validate against Unity Dev Basics patterns
- Create Bootstrap Scene Asset (CreateAsset, S)
  - Create new Unity scene file
  - Set up Canvas with appropriate render mode
  - Configure EventSystem for input handling
  - Add placeholder UI elements
  - Save scene in proper location
- Implement Splash Display Logic (Code, S)
  - Create MonoBehaviour for splash control
  - Implement 2-second minimum display timer
  - Add smooth transition animations
  - Handle scene transition after splash
  - Follow composition over inheritance principles
- Configure TextMeshPro Integration (Config, XS)
  - Import TextMeshPro package
  - Configure default font assets
  - Set up UI text components
  - Validate text rendering on all platforms
- [Play-mode Test] Bootstrap Scene Tests (Test, S)
  - Test scene loads correctly on all platforms
  - Verify splash display duration
  - Test UI scaling across resolutions
  - Validate smooth transitions
  - Ensure tests run in CI/CD pipeline

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

#### Tasks

- Design Permission Request Flow (Design, S)
  - Define permission request timing and user experience
  - Plan graceful handling of permission denial
  - Design user feedback for permission states
  - Document platform-specific permission behaviors
  - Validate against platform best practices
- Configure Android Manifest Permissions (Config, XS)
  - Add RECORD_AUDIO permission to AndroidManifest.xml
  - Configure permission request rationale
  - Set appropriate permission protection level
  - Validate manifest syntax
- Configure iOS Info.plist Permissions (Config, XS)
  - Add NSMicrophoneUsageDescription to Info.plist
  - Write clear user-facing permission description
  - Validate plist syntax and compliance
  - Test permission prompt appearance
- Implement Permission Request Logic (Code, M)
  - Create permission manager component
  - Implement runtime permission requests for Android
  - Handle iOS permission status checking
  - Add user feedback for permission denial
  - Implement permission status persistence
  - Follow composition over inheritance principles
- [Edit-mode Test] Permission Handler Tests (Test, S)
  - Test permission status checking logic
  - Verify permission request flow
  - Test error handling for permission failures
  - Validate platform-specific behavior
  - Ensure tests run in CI/CD pipeline
- [Play-mode Test] Permission Integration Tests (Test, S)
  - Test actual permission requests on device
  - Verify user feedback displays correctly
  - Test app behavior with denied permissions
  - Validate permission persistence
  - Test across multiple app launches

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

#### Tasks

- Design Audio Device Detection System (Design, S)
  - Define microphone device enumeration strategy
  - Plan platform-specific device handling
  - Design error handling for missing devices
  - Document device selection criteria
  - Validate against GDD audio requirements
- Implement Microphone Device Enumeration (Code, M)
  - Use Unity Microphone.devices API
  - Create device information collection
  - Implement device availability checking
  - Add device selection logic
  - Follow composition over inheritance principles
- Add Platform-Specific Audio Handling (Code, M)
  - Implement Windows WASAPI default settings
  - Handle Android audio device variations
  - Manage iOS audio session configuration
  - Add platform-specific optimizations
  - Validate audio input capabilities
- Implement Device Detection Error Handling (Code, S)
  - Handle empty device list gracefully
  - Provide user feedback for audio issues
  - Implement fallback device selection
  - Add audio troubleshooting guidance
  - Log device detection failures
- [Edit-mode Test] Audio Device Detection Tests (Test, S)
  - Test device enumeration logic
  - Verify error handling paths
  - Test platform-specific behavior
  - Validate device selection criteria
  - Ensure tests run in CI/CD pipeline
- [Play-mode Test] Microphone Integration Tests (Test, M)
  - Test actual microphone device detection
  - Verify audio input capability
  - Test platform-specific audio behavior
  - Validate error handling on real devices
  - Test across different hardware configurations

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

#### Tasks

- Design Platform Validator Architecture (Design, S)
  - Define validation component structure
  - Plan capability checking strategy
  - Design validation result reporting
  - Document component lifecycle
  - Validate against Unity Dev Basics patterns
- Implement PlatformValidator Component (Code, M)
  - Create MonoBehaviour validator component
  - Implement device capability checking
  - Add platform-specific validation logic
  - Integrate with microphone detection
  - Follow composition over inheritance principles
- Add Validation Result Logging (Code, S)
  - Implement structured logging system
  - Log validation results for debugging
  - Add performance metrics collection
  - Create validation result storage
  - Format logs for easy analysis
- Implement Graceful Failure Handling (Code, S)
  - Handle validation failures gracefully
  - Provide actionable user feedback
  - Implement fallback behaviors
  - Add validation retry mechanisms
  - Log failure details for troubleshooting
- [Edit-mode Test] Platform Validator Tests (Test, S)
  - Test validation logic components
  - Verify error handling paths
  - Test logging functionality
  - Validate component lifecycle
  - Ensure tests run in CI/CD pipeline
- [Play-mode Test] Validation Integration Tests (Test, S)
  - Test validator component in runtime
  - Verify validation results accuracy
  - Test user feedback mechanisms
  - Validate graceful failure handling
  - Test across different device configurations

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

#### Tasks

- Design BuildConfig ScriptableObject Schema (Design, S)
  - Define configuration data structure
  - Plan platform-specific settings organization
  - Design configuration validation rules
  - Document configuration usage patterns
  - Validate against Unity Dev Basics patterns
- Create BuildConfig ScriptableObject (CreateAsset, S)
  - Implement ScriptableObject class definition
  - Add serialized fields for build settings
  - Implement configuration validation methods
  - Add editor custom property drawer
  - Create base configuration template
- Create Platform-Specific Config Assets (CreateAsset, M)
  - Create Android build configuration asset
  - Create iOS build configuration asset
  - Create Windows build configuration asset
  - Configure platform-specific settings values
  - Validate configuration completeness
  - Set up asset organization structure
- Implement Config Asset Loading (Code, S)
  - Create configuration loader component
  - Implement runtime config asset loading
  - Add configuration validation at runtime
  - Handle missing configuration gracefully
  - Follow composition over inheritance principles
- [Edit-mode Test] BuildConfig Asset Tests (Test, S)
  - Test ScriptableObject creation and serialization
  - Verify configuration validation logic
  - Test asset loading mechanisms
  - Validate platform-specific configurations
  - Ensure tests run in CI/CD pipeline

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

#### Tasks

- Design Test Suite Architecture (Design, S)
  - Define test organization and structure
  - Plan edit-mode vs play-mode test distribution
  - Design test data management strategy
  - Document testing patterns and conventions
  - Validate against Testing in Unity guide
- Create Edit-Mode Test Framework (Code, M)
  - Set up Unity Test Framework for edit-mode
  - Create base test classes and utilities
  - Implement mock objects for platform testing
  - Add test data generation helpers
  - Configure test assembly definitions
- Create Play-Mode Test Framework (Code, M)
  - Set up Unity Test Framework for play-mode
  - Create scene-based test infrastructure
  - Implement device simulation utilities
  - Add integration test helpers
  - Configure test runner settings
- Implement Platform-Specific Test Cases (Code, M)
  - Create Android-specific test cases
  - Create iOS-specific test cases
  - Create Windows-specific test cases
  - Add cross-platform compatibility tests
  - Implement performance baseline tests
  - Add audio device detection tests
- Configure CI/CD Test Integration (Config, S)
  - Set up test execution in build pipeline
  - Configure test result reporting
  - Add test failure notifications
  - Set up test coverage reporting
  - Configure test artifact storage

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

#### Tasks

- Design CI/CD Pipeline Architecture (Design, M)
  - Define build pipeline stages and dependencies
  - Plan artifact management strategy
  - Design version tagging and release process
  - Document pipeline configuration requirements
  - Validate against project deployment needs
- Configure GitHub Actions Workflow (Config, M)
  - Create workflow configuration files
  - Set up Unity licensing for CI/CD
  - Configure build matrix for multiple platforms
  - Add test execution stages
  - Set up artifact upload and storage
- Implement Android Build Automation (Code, M)
  - Configure Android build parameters
  - Set up APK/AAB generation
  - Add signing configuration
  - Implement build validation checks
  - Configure artifact naming and storage
- Implement iOS Build Automation (Code, M)
  - Configure iOS build parameters
  - Set up IPA generation workflow
  - Add certificate and provisioning handling
  - Implement build validation checks
  - Configure artifact naming and storage
- Implement Windows Build Automation (Code, S)
  - Configure Windows build parameters
  - Set up executable generation
  - Add build validation checks
  - Configure artifact naming and storage
  - Test build output structure
- Configure Build Artifact Management (Config, S)
  - Set up artifact storage and retention
  - Configure version tagging strategy
  - Add build metadata collection
  - Set up artifact download mechanisms
  - Configure cleanup and archival policies
