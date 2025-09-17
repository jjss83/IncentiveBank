# Platform Configuration Test Suite

## Overview

This test suite validates Unity configuration settings for Windows platform builds, specifically covering requirements from **MBA-32: Configure Windows Build Settings**.

## Test Structure

### Edit-Mode Tests (`Assets/Tests/EditMode/`)

All tests run in Unity's edit-mode and validate configuration without requiring a build.

#### `PlatformConfigurationTests.cs`
- **Bundle Identifier**: Validates Windows bundle ID format (`com.sandez.incentivebank.windows`)
- **Company Name**: Ensures company name is set to "Sandez Games"
- **Product Name**: Validates executable will be named "IncentiveBank.exe"
- **Build Numbers**: Checks version consistency across platforms
- **Bundle Version**: Validates semantic versioning format

#### `GraphicsAndArchitectureTests.cs`
- **x64 Architecture**: Validates Windows builds target 64-bit systems only
- **Graphics APIs**: Ensures DirectX 11 priority with Vulkan/OpenGL fallbacks
- **Manual Configuration**: Validates automatic graphics API selection is disabled
- **Scripting Backend**: Checks appropriate scripting backend configuration
- **Color Space**: Validates Linear or Gamma color space configuration

#### `AudioConfigurationTests.cs`
- **WASAPI Defaults**: Validates sample rate is device default (0) per GDD
- **DSP Buffer Size**: Ensures appropriate buffer size for real-time audio
- **Speaker Configuration**: Validates speaker mode settings
- **Voice Counts**: Checks sufficient virtual and real voice allocation
- **Microphone Access**: Validates Unity Microphone API accessibility
- **Audio System**: Ensures audio is not disabled for VAD functionality

#### `BuildOutputStructureTests.cs`
- **Executable Naming**: Validates "IncentiveBank.exe" generation
- **Bundle ID Format**: Ensures reverse domain notation compliance
- **Version Consistency**: Validates version synchronization across settings
- **Data Folder**: Validates "IncentiveBank_Data" folder generation
- **File Metadata**: Ensures proper company/product metadata embedding
- **Build Targets**: Validates Windows build target accessibility

#### `PlatformConfigurationTestSuite.cs`
- **Coverage Documentation**: Documents complete test suite structure
- **Framework Validation**: Ensures NUnit integration works correctly

## Running Tests

### Unity Editor
1. Open Unity Test Runner: `Window > General > Test Runner`
2. Select "EditMode" tab
3. Click "Run All" to execute complete test suite
4. Individual test classes can be run by expanding and selecting specific tests

### Command Line
```bash
# Run all edit-mode tests
Unity.exe -batchmode -runTests -testPlatform EditMode -testResults results.xml

# Run specific test class
Unity.exe -batchmode -runTests -testPlatform EditMode -testFilter "PlatformConfigurationTests"
```

### CI/CD Integration
Tests are designed to run in automated CI/CD pipelines and will validate configuration consistency across builds.

## Test Coverage

### MBA-32 Requirements ✅
- [x] Set minimum Windows version to 10+ (via x64 architecture)
- [x] Configure x64 architecture 
- [x] Set appropriate graphics API (DirectX/OpenGL)
- [x] Configure executable and data folder structure
- [x] Validate WASAPI audio settings per GDD

### GDD Compliance ✅
- [x] "Windows: keep default WASAPI settings" - Audio tests validate default sample rate
- [x] Bundle identifier format validation
- [x] Company and product naming consistency
- [x] Version management across platforms

## Maintenance

### Adding New Tests
1. Create test methods in appropriate test class based on category
2. Follow naming convention: `Feature_Scenario_Expectation`
3. Add AI comments explaining test purpose and MBA-32 traceability
4. Use AAA pattern (Arrange-Act-Assert)

### Updating Configuration
When Unity configuration changes, corresponding tests should be updated to validate new requirements.

## Dependencies

- **Unity Test Framework**: Provides NUnit integration and test runner
- **UnityEditor**: Required for accessing PlayerSettings and build configuration
- **NUnit Framework**: Testing assertions and attributes

## Notes

- Tests validate configuration state, not build execution
- All tests are deterministic and run without external dependencies
- Test failures indicate configuration drift from MBA-32 requirements
- Tests serve as living documentation of platform configuration decisions