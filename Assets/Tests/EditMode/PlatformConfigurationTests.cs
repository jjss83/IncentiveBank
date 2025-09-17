using NUnit.Framework;
using UnityEditor;

namespace IncentiveBank.Tests.EditMode
{
    /// <summary>
    /// Edit-mode tests for validating Unity platform configuration settings.
    /// Tests Windows platform configuration applied in MBA-32.
    /// </summary>
    [TestFixture]
    public class PlatformConfigurationTests
    {
        /// <summary>
        /// AI: Purpose: Validate Windows bundle identifier format and consistency.
        /// Ensures bundle ID follows convention and matches Windows platform requirements.
        /// </summary>
        [Test]
        public void WindowsBundleIdentifier_HasCorrectFormat()
        {
            // Arrange
            const string expectedBundleId = "com.sandez.incentivebank.windows";
            
            // Act
#pragma warning disable CS0618 // Type or member is obsolete
            string actualBundleId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Standalone);
#pragma warning restore CS0618
            
            // Assert
            Assert.That(actualBundleId, Is.EqualTo(expectedBundleId), 
                "Windows bundle identifier should match expected format for MBA-32");
        }

        /// <summary>
        /// AI: Purpose: Validate company name is properly set for Windows builds.
        /// Ensures professional metadata in generated executables.
        /// </summary>
        [Test]
        public void CompanyName_IsSetCorrectly()
        {
            // Arrange
            const string expectedCompanyName = "Sandez Games";
            
            // Act
            string actualCompanyName = PlayerSettings.companyName;
            
            // Assert
            Assert.That(actualCompanyName, Is.EqualTo(expectedCompanyName),
                "Company name should be set to 'Sandez Games' for professional branding");
        }

        /// <summary>
        /// AI: Purpose: Validate product name matches expected executable naming.
        /// Ensures Windows executable will be named 'IncentiveBank.exe'.
        /// </summary>
        [Test]
        public void ProductName_MatchesExpectedExecutableName()
        {
            // Arrange
            const string expectedProductName = "IncentiveBank";
            
            // Act
            string actualProductName = PlayerSettings.productName;
            
            // Assert
            Assert.That(actualProductName, Is.EqualTo(expectedProductName),
                "Product name should match expected executable name");
        }

        /// <summary>
        /// AI: Purpose: Validate build number consistency across standalone platforms.
        /// Ensures version consistency for Windows builds.
        /// </summary>
        [Test]
        public void BundleVersion_IsSetForStandalone()
        {
            // Arrange
            const string expectedVersionPattern = @"^\d+\.\d+";
            
            // Act
            string bundleVersion = PlayerSettings.bundleVersion;
            
            // Assert
            Assert.That(bundleVersion, Does.Match(expectedVersionPattern),
                "Bundle version should follow major.minor format for Windows distribution");
        }

        /// <summary>
        /// AI: Purpose: Validate bundle version format for release management.
        /// Ensures proper versioning for Windows distribution.
        /// </summary>
        [Test]
        public void BundleVersion_HasValidFormat()
        {
            // Act
            string bundleVersion = PlayerSettings.bundleVersion;
            
            // Assert
            Assert.That(bundleVersion, Is.Not.Null.And.Not.Empty,
                "Bundle version should not be null or empty");
            Assert.That(bundleVersion, Does.Match(@"^\d+\.\d+"),
                "Bundle version should follow major.minor format pattern");
        }
    }
}