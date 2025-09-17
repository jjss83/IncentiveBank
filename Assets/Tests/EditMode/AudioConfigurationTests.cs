using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace IncentiveBank.Tests.EditMode
{
    /// <summary>
    /// Edit-mode tests for validating Unity audio configuration.
    /// Tests WASAPI audio settings per GDD requirements for Windows platform.
    /// </summary>
    [TestFixture]
    public class AudioConfigurationTests
    {
        /// <summary>
        /// AI: Purpose: Validate WASAPI default sample rate configuration.
        /// Ensures runtime sample rate resolves to a standard device default (44100 or 48000)
        /// when configured to use the device default per GDD "keep default WASAPI settings".
        /// </summary>
        [Test]
        public void AudioSampleRate_IsDeviceDefault()
        {
            // Act
            var audioConfiguration = AudioSettings.GetConfiguration();
            
            // Assert
            var expectedDefaults = new[] { 44100, 48000 };
            Assert.That(expectedDefaults, Contains.Item(audioConfiguration.sampleRate),
                "Sample rate should resolve to a standard device default (44100 or 48000) on WASAPI");
        }

        /// <summary>
        /// AI: Purpose: Validate DSP buffer size is appropriate for Windows audio latency.
        /// Ensures buffer size supports real-time microphone input for VAD functionality.
        /// </summary>
        [Test]
        public void AudioDSPBufferSize_IsAppropriateForRealTime()
        {
            // Act
            var audioConfiguration = AudioSettings.GetConfiguration();
            
            // Assert
            Assert.That(audioConfiguration.dspBufferSize, Is.GreaterThan(0),
                "DSP buffer size should be positive for audio processing");
            Assert.That(audioConfiguration.dspBufferSize, Is.LessThanOrEqualTo(4096),
                "DSP buffer size should not exceed 4096 for reasonable latency");
        }

        /// <summary>
        /// AI: Purpose: Validate audio speaker configuration is appropriate.
        /// Ensures speaker mode supports typical Windows audio setups.
        /// </summary>
        [Test]
        public void AudioSpeakerMode_IsConfiguredAppropriately()
        {
            // Act
            var audioConfiguration = AudioSettings.GetConfiguration();
            
            // Assert
            var validSpeakerModes = new[] {
                AudioSpeakerMode.Mono, 
                AudioSpeakerMode.Stereo, 
                AudioSpeakerMode.Quad, 
                AudioSpeakerMode.Surround, 
                AudioSpeakerMode.Mode5point1, 
                AudioSpeakerMode.Mode7point1
            };
            
            Assert.That(validSpeakerModes, Contains.Item(audioConfiguration.speakerMode),
                "Speaker mode should be a valid audio configuration");
        }

        /// <summary>
        /// AI: Purpose: Validate virtual voice count supports audio complexity.
        /// Ensures sufficient virtual voices for UI sounds and microphone processing.
        /// </summary>
        [Test]
        public void AudioVirtualVoiceCount_IsSufficient()
        {
            // Arrange
            const int minimumVirtualVoices = 32;
            
            // Act
            var audioConfiguration = AudioSettings.GetConfiguration();
            
            // Assert
            Assert.That(audioConfiguration.numVirtualVoices, Is.GreaterThanOrEqualTo(minimumVirtualVoices),
                "Virtual voice count should support UI sounds and audio processing");
        }

        /// <summary>
        /// AI: Purpose: Validate real voice count supports simultaneous audio sources.
        /// Ensures enough real voices for microphone input and feedback sounds.
        /// </summary>
        [Test]
        public void AudioRealVoiceCount_IsSufficient()
        {
            // Arrange
            const int minimumRealVoices = 16;
            
            // Act
            var audioConfiguration = AudioSettings.GetConfiguration();
            
            // Assert
            Assert.That(audioConfiguration.numRealVoices, Is.GreaterThanOrEqualTo(minimumRealVoices),
                "Real voice count should support microphone and UI audio simultaneously");
        }

        /// <summary>
        /// AI: Purpose: Validate microphone device enumeration capability.
        /// Ensures Unity can detect available microphone devices for VAD functionality.
        /// </summary>
        [Test]
        public void MicrophoneDevices_CanBeEnumerated()
        {
            // Act
            string[] devices = Microphone.devices;
            
            // Assert
            Assert.That(devices, Is.Not.Null,
                "Microphone.devices should not return null");
            
            // Note: Device array may be empty in test environment, but API should work
            // This test validates the API is accessible, not that hardware is present
        }

        /// <summary>
        /// AI: Purpose: Validate audio system initialization state.
        /// Ensures audio system is properly configured and not disabled.
        /// </summary>
        [Test]
        public void AudioSystem_IsNotDisabled()
        {
            // Act
            var audioConfiguration = AudioSettings.GetConfiguration();
            bool audioDisabled = AudioSettings.GetConfiguration().sampleRate == 0 && 
                                 AudioSettings.GetConfiguration().dspBufferSize == 0;
            
            // Assert
            Assert.That(audioDisabled, Is.False,
                "Audio system should not be completely disabled for microphone functionality");
        }

        /// <summary>
        /// AI: Purpose: Validate audio settings support WASAPI requirements.
        /// Ensures configuration aligns with GDD requirement to "keep default WASAPI settings".
        /// </summary>
        [Test]
        public void AudioConfiguration_SupportsWASAPIDefaults()
        {
            // Act
            var audioConfiguration = AudioSettings.GetConfiguration();
            
            // Assert
            var validSampleRates = new[] { 0, 44100, 48000 };
            Assert.That(validSampleRates, Contains.Item(audioConfiguration.sampleRate),
                "Sample rate should be device default (0) or standard audio rates");
            
            var validBufferSizes = new[] { 256, 512, 1024, 2048 };
            Assert.That(validBufferSizes, Contains.Item(audioConfiguration.dspBufferSize),
                "DSP buffer size should be standard power-of-2 value for WASAPI compatibility");
        }
    }
}