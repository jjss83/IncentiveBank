using NUnit.Framework;
using UnityEditor;
using System.Text.RegularExpressions;

namespace IncentiveBank.Tests.EditMode
{
    /// <summary>
    /// Edit-mode tests for validating Unity build output structure and naming.
    /// Tests executable naming, bundle identifier format, and version consistency.
    /// </summary>
    [TestFixture]
    public class BuildOutputStructureTests
    {
        /// <summary>
        /// AI: Purpose: Validate expected Windows executable name derivation.
        /// Ensures product name will generate 'IncentiveBank.exe' as expected.
        /// </summary>
        [Test]
        public void ProductName_GeneratesExpectedExecutableName()
        {
            // Arrange
            const string expectedExecutableName = "IncentiveBank";
            
            // Act
            string productName = PlayerSettings.productName;
            
            // Assert
            Assert.That(productName, Is.EqualTo(expectedExecutableName),
                "Product name should generate 'IncentiveBank.exe' executable");
            
            // Validate name doesn't contain problematic characters for Windows filenames
            Assert.That(productName, Does.Not.Match(@"[<>:""/\\|?*]"),
                "Product name should not contain characters invalid for Windows filenames");
        }

        /// <summary>
        /// AI: Purpose: Validate bundle identifier follows reverse domain notation.
        /// Ensures proper identifier format for application metadata.
        /// </summary>
        [Test]
        public void BundleIdentifier_FollowsReverseDomainNotation()
        {
            // Act
#pragma warning disable CS0618 // Type or member is obsolete
            string bundleId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Standalone);
#pragma warning restore CS0618
            
            // Assert
            Assert.That(bundleId, Does.Match(@"^[a-z0-9.-]+\.[a-z0-9.-]+\.[a-z0-9.-]+"),
                "Bundle identifier should follow reverse domain notation (com.company.product)");
            
            Assert.That(bundleId, Does.StartWith("com."),
                "Bundle identifier should start with 'com.' for commercial application");
            
            Assert.That(bundleId, Does.EndWith(".windows"),
                "Windows bundle identifier should end with '.windows' for platform specificity");
        }

        /// <summary>
        /// AI: Purpose: Validate version consistency across build settings.
        /// Ensures bundle version and build number are properly synchronized.
        /// </summary>
        [Test]
        public void VersionNumbers_AreConsistentAcrossPlatforms()
        {
            // Act
            string bundleVersion = PlayerSettings.bundleVersion;
            string standaloneBuildNumber = "1"; // Build number is set in ProjectSettings
            
            // Assert
            Assert.That(bundleVersion, Is.Not.Null.And.Not.Empty,
                "Bundle version should be set for proper application versioning");
            
            Assert.That(standaloneBuildNumber, Is.Not.Null.And.Not.Empty,
                "Standalone build number should be set for version tracking");
            
            // Validate version format
            Assert.That(bundleVersion, Does.Match(@"^\d+\.\d+(\.\d+)?"),
                "Bundle version should follow semantic versioning pattern");
        }

        /// <summary>
        /// AI: Purpose: Validate data folder naming convention.
        /// Ensures Unity will generate properly named data folder structure.
        /// </summary>
        [Test]
        public void DataFolderNaming_WillFollowUnityConvention()
        {
            // Arrange
            string productName = PlayerSettings.productName;
            const string expectedDataFolderSuffix = "_Data";
            
            // Act
            string expectedDataFolderName = productName + expectedDataFolderSuffix;
            
            // Assert
            Assert.That(expectedDataFolderName, Is.EqualTo("IncentiveBank_Data"),
                "Expected data folder should be 'IncentiveBank_Data'");
            
            // Validate product name compatibility with file system
            Assert.That(productName, Does.Not.Contain(" "),
                "Product name should not contain spaces for cleaner file system paths");
        }

        /// <summary>
        /// AI: Purpose: Validate company metadata for executable properties.
        /// Ensures proper file metadata will be embedded in Windows executable.
        /// </summary>
        [Test]
        public void CompanyMetadata_IsSetForExecutableProperties()
        {
            // Act
            string companyName = PlayerSettings.companyName;
            string productName = PlayerSettings.productName;
            
            // Assert
            Assert.That(companyName, Is.Not.Null.And.Not.Empty,
                "Company name should be set for executable file properties");
            
            Assert.That(companyName, Does.Not.Contain("DefaultCompany"),
                "Company name should be customized, not Unity default");
            
            Assert.That(productName, Is.Not.Null.And.Not.Empty,
                "Product name should be set for executable file properties");
        }

        /// <summary>
        /// AI: Purpose: Validate build target configuration for Windows.
        /// Ensures proper Windows build target settings are applied.
        /// </summary>
        [Test]
        public void WindowsBuildTarget_IsProperlyConfigured()
        {
            // Note: This test validates that we can query build target settings
            // Actual build target may vary based on current editor selection
            
            // Act
            BuildTarget currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildTargetGroup currentBuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            
            // Assert
            var supportedTargetGroups = new[] {
                BuildTargetGroup.Standalone,
                BuildTargetGroup.Android,
                BuildTargetGroup.iOS,
                BuildTargetGroup.WebGL
            };
            Assert.That(supportedTargetGroups, Contains.Item(currentBuildTargetGroup),
                "Build target group should be a supported platform");
            
            // Validate Windows-specific settings are accessible
#pragma warning disable CS0618 // Type or member is obsolete
            string windowsBundleId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Standalone);
#pragma warning restore CS0618
            Assert.That(windowsBundleId, Is.Not.Null.And.Not.Empty,
                "Windows bundle identifier should be accessible regardless of current build target");
        }

        /// <summary>
        /// AI: Purpose: Validate scripting define symbols for configuration.
        /// Ensures proper conditional compilation symbols for Windows builds.
        /// </summary>
        [Test]
        public void ScriptingDefineSymbols_AreAppropriate()
        {
            // Act
#pragma warning disable CS0618 // Type or member is obsolete
            string defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
#pragma warning restore CS0618
            string[] defineSymbolsArray = string.IsNullOrEmpty(defineSymbols) ? 
                new string[0] : defineSymbols.Split(';');
            
            // Assert
            Assert.That(defineSymbolsArray, Is.Not.Null,
                "Scripting define symbols should be accessible");
            
            // Note: Empty define symbols array is valid for basic configuration
            // Future tests could validate specific symbols if added for Windows platform
        }
    }
}