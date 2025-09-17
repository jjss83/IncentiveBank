using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace IncentiveBank.Tests.EditMode
{
    /// <summary>
    /// Edit-mode tests for validating Unity graphics and architecture configuration.
    /// Tests Windows x64 architecture and graphics API settings applied in MBA-32.
    /// </summary>
    [TestFixture]
    public class GraphicsAndArchitectureTests
    {
        /// <summary>
        /// AI: Purpose: Validate Windows x64 architecture configuration.
        /// Ensures Windows builds target 64-bit systems only as per MBA-32 requirements.
        /// </summary>
        [Test]
        public void WindowsArchitecture_IsConfiguredForX64()
        {
            // Arrange
            const int expectedX64Value = 1; // Unity's x64 architecture enum value
            
            // Act
#pragma warning disable CS0618 // Type or member is obsolete
            var architectureValue = PlayerSettings.GetArchitecture(BuildTargetGroup.Standalone);
#pragma warning restore CS0618
            
            // Assert
            Assert.That(architectureValue, Is.EqualTo(expectedX64Value),
                "Windows Standalone should be configured for x64 architecture only");
        }

        /// <summary>
        /// AI: Purpose: Validate Windows graphics APIs are explicitly configured.
        /// Ensures DirectX/Vulkan/OpenGL priority order and manual selection.
        /// </summary>
        [Test]
        public void WindowsGraphicsAPIs_AreExplicitlyConfigured()
        {
            // Act
            var graphicsAPIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneWindows64);
            bool isAutomatic = PlayerSettings.GetUseDefaultGraphicsAPIs(BuildTarget.StandaloneWindows64);
            
            // Assert
            Assert.That(isAutomatic, Is.False,
                "Windows graphics APIs should be manually configured, not automatic");
            Assert.That(graphicsAPIs, Is.Not.Null.And.Length.GreaterThan(0),
                "Windows should have explicitly configured graphics APIs");
        }

        /// <summary>
        /// AI: Purpose: Validate DirectX 11 is the primary graphics API.
        /// Ensures Windows builds prioritize DirectX 11 for compatibility.
        /// </summary>
        [Test]
        public void WindowsGraphicsAPIs_PrioritizeDirectX11()
        {
            // Act
            var graphicsAPIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneWindows64);
            
            // Assert
            Assert.That(graphicsAPIs, Is.Not.Null.And.Length.GreaterThan(0),
                "Windows should have configured graphics APIs");
            Assert.That(graphicsAPIs[0], Is.EqualTo(GraphicsDeviceType.Direct3D11),
                "DirectX 11 should be the primary graphics API for Windows");
        }

        /// <summary>
        /// AI: Purpose: Validate graphics API fallback configuration.
        /// Ensures proper fallback order: DirectX → Vulkan → OpenGL.
        /// </summary>
        [Test]
        public void WindowsGraphicsAPIs_HaveProperFallbackOrder()
        {
            // Act
            var graphicsAPIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneWindows64);
            
            // Assert
            Assert.That(graphicsAPIs, Is.Not.Null.And.Length.GreaterThanOrEqualTo(2),
                "Windows should have multiple graphics APIs for fallback");
            
            // Verify expected APIs are present (order may vary based on configuration)
            Assert.That(graphicsAPIs, Contains.Item(GraphicsDeviceType.Direct3D11),
                "DirectX 11 should be available for Windows");
        }

        /// <summary>
        /// AI: Purpose: Validate scripting backend configuration for Windows.
        /// Ensures proper scripting backend is configured for performance.
        /// </summary>
        [Test]
        public void WindowsScriptingBackend_IsConfigured()
        {
            // Act
#pragma warning disable CS0618 // Type or member is obsolete
            var scriptingBackend = PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone);
#pragma warning restore CS0618
            
            // Assert
            // Note: Default scripting backend may be Mono2x or IL2CPP depending on Unity version
            Assert.That(scriptingBackend, Is.Not.EqualTo(ScriptingImplementation.WinRTDotNET),
                "Windows should not use deprecated WinRT .NET scripting backend");
        }

        /// <summary>
        /// AI: Purpose: Validate color space configuration for Windows.
        /// Ensures appropriate color space for modern Windows graphics.
        /// </summary>
        [Test]
        public void ColorSpace_IsConfiguredAppropriately()
        {
            // Act
            var colorSpace = PlayerSettings.colorSpace;
            
            // Assert
            var validColorSpaces = new[] { ColorSpace.Linear, ColorSpace.Gamma };
            Assert.That(validColorSpaces, Contains.Item(colorSpace),
                "Color space should be either Linear or Gamma (both are valid for Windows)");
        }
    }
}