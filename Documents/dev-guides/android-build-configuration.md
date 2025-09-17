# Android Build Configuration Guide

## Overview
This document outlines the Android build configuration for the IncentiveBank Unity project, ensuring optimal performance and compatibility with modern Android devices.

## Configuration Details

### 1. API Level Configuration ✅
- **Minimum SDK Version**: 23 (Android 6.0 Marshmallow)
- **Target SDK Version**: 34 (Android 14)
- **Rationale**: API 23+ ensures compatibility with modern Android features while maintaining reasonable device coverage

### 2. Architecture Configuration ✅
- **Target Architecture**: ARM64 only
- **Scripting Backend**: IL2CPP
- **Rationale**: ARM64 is required for Google Play Store submissions and provides better performance

### 3. Texture Compression ✅
- **Format**: ASTC (Adaptive Scalable Texture Compression)
- **Graphics APIs**: Automatic (Vulkan/OpenGL ES)
- **Rationale**: ASTC provides the best balance of quality and performance across modern Android devices

### 4. Signing and Security Configuration ✅
- **Custom Keystore**: Enabled
- **Keystore Name**: user.keystore
- **Key Alias**: incentivebank
- **Bundle Identifier**: com.incentivebank.app
- **Security Features**: 
  - ARMv9 Security Features: Enabled
  - ARM64 MTE (Memory Tagging Extension): Enabled

### 5. Build Settings ✅
- **Scenes in Build**:
  - SplashScene.unity (Scene 0)
  - SampleScene.unity (Scene 1)
- **Static Batching**: Enabled
- **Dynamic Batching**: Disabled (recommended for mobile)

## Security Configuration

### Keystore Setup
Before building for release, you need to create a keystore file:

```bash
# Create a new keystore (run this once)
keytool -genkey -v -keystore user.keystore -alias incentivebank -keyalg RSA -keysize 2048 -validity 10000

# Store the keystore file in a secure location
# Never commit the keystore file to version control
```

### Important Security Notes
1. **Keystore Password**: Store securely and never commit to repository
2. **Key Alias Password**: Use a strong, unique password
3. **Backup**: Keep secure backups of your keystore file
4. **CI/CD**: Use environment variables for passwords in automated builds

## Build Output Structure

### Expected APK/AAB Contents
```
IncentiveBank.apk/
├── AndroidManifest.xml
├── classes.dex
├── resources.arsc
├── lib/
│   └── arm64-v8a/
│       ├── libil2cpp.so
│       ├── libunity.so
│       └── libmain.so
├── assets/
│   ├── aa/
│   ├── bin/
│   └── sharedassets0.assets
└── META-INF/
    ├── MANIFEST.MF
    ├── CERT.SF
    └── CERT.RSA
```

### Validation Checklist
- [ ] APK size < 150MB (Google Play requirement)
- [ ] ARM64 native libraries present
- [ ] Proper signing certificates
- [ ] Target SDK compliance
- [ ] Permission declarations minimal

## Performance Optimizations

### Quality Settings
- **Android Default Quality**: Medium (index 2)
- **Texture Quality**: Full resolution
- **Anisotropic Filtering**: Per Texture
- **Anti-aliasing**: Disabled (for performance)

### Build Size Optimization
- **Texture Compression**: ASTC
- **Strip Engine Code**: Enabled
- **Vertex Compression**: Enabled for position, normal, and UV channels
- **IL2CPP Code Generation**: Optimized for size and speed

## Troubleshooting

### Common Issues
1. **Build Size Too Large**
   - Enable texture compression
   - Use Asset Bundles for large assets
   - Strip unused assets

2. **Performance Issues**
   - Verify ARM64 native libraries
   - Check quality settings
   - Profile on target devices

3. **Store Rejection**
   - Ensure target SDK version is current
   - Verify all required permissions
   - Test on various device configurations

## CI/CD Integration

### Environment Variables
```bash
ANDROID_KEYSTORE_PATH=/path/to/user.keystore
ANDROID_KEYSTORE_PASS=your_keystore_password
ANDROID_KEY_ALIAS=incentivebank
ANDROID_KEY_PASS=your_key_password
```

### Build Command Example
```bash
Unity -batchmode -quit -projectPath . -buildTarget Android -buildPath ./Builds/IncentiveBank.apk
```

## Next Steps
1. Create and configure the actual keystore file
2. Test build on various Android devices
3. Set up automated CI/CD pipeline
4. Configure Google Play Console for distribution