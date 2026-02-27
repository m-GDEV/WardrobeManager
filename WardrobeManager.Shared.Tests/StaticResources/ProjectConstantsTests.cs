using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Shared.StaticResources;

namespace WardrobeManager.Shared.Tests.StaticResources;

public class ProjectConstantsTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ProjectName_HasExpectedValue()
    {
        // Arrange
        // Act
        // Assert
        ProjectConstants.ProjectName.Should().Be("Wardrobe Manager");
    }

    [Test]
    public void ProjectVersion_FollowsSemVer()
    {
        // Arrange
        // Act
        var version = ProjectConstants.ProjectVersion;

        // Assert
        // SemVer format: MAJOR.MINOR.PATCH
        var parts = version.Split('.');
        using (new AssertionScope())
        {
            parts.Should().HaveCount(3);
            parts[0].Should().MatchRegex(@"^\d+$");
            parts[1].Should().MatchRegex(@"^\d+$");
            parts[2].Should().MatchRegex(@"^\d+$");
        }
    }

    [Test]
    public void ProjectGitRepo_ContainsProjectName()
    {
        // Arrange
        var expectedRepoName = ProjectConstants.ProjectName.Replace(" ", "");

        // Act
        var repoUrl = ProjectConstants.ProjectGitRepo;

        // Assert
        repoUrl.Should().Contain(expectedRepoName);
    }

    [Test]
    public void ProjectGitRepo_IsValidUrl()
    {
        // Arrange
        // Act
        var repoUrl = ProjectConstants.ProjectGitRepo;

        // Assert
        repoUrl.Should().StartWith("https://github.com/");
    }

    [Test]
    public void DefaultItemImage_IsRelativePath()
    {
        // Arrange
        // Act
        // Assert
        ProjectConstants.DefaultItemImage.Should().StartWith("/");
    }

    [Test]
    public void HomeBackgroundImage_IsRelativePath()
    {
        // Arrange
        // Act
        // Assert
        ProjectConstants.HomeBackgroundImage.Should().StartWith("/");
    }
}
