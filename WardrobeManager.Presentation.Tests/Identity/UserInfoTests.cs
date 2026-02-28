using FluentAssertions;
using FluentAssertions.Execution;
using WardrobeManager.Presentation.Identity.Models;

namespace WardrobeManager.Presentation.Tests.Identity;

public class UserInfoTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void UserInfo_WhenCreated_HasDefaultValues()
    {
        // Arrange
        // Act
        var userInfo = new UserInfo();

        // Assert
        using (new AssertionScope())
        {
            userInfo.Email.Should().Be(string.Empty);
            userInfo.IsEmailConfirmed.Should().BeFalse();
            userInfo.Claims.Should().BeEmpty();
        }
    }

    [Test]
    public void UserInfo_WhenEmailIsSet_ReturnsSetEmail()
    {
        // Arrange
        const string email = "test@example.com";

        // Act
        var userInfo = new UserInfo { Email = email };

        // Assert
        userInfo.Email.Should().Be(email);
    }

    [Test]
    public void UserInfo_WhenIsEmailConfirmedIsSet_ReturnsSetValue()
    {
        // Arrange
        // Act
        var userInfo = new UserInfo { IsEmailConfirmed = true };

        // Assert
        userInfo.IsEmailConfirmed.Should().BeTrue();
    }

    [Test]
    public void UserInfo_WhenClaimsAreAdded_ContainsClaims()
    {
        // Arrange
        var claims = new Dictionary<string, string>
        {
            { "role", "Admin" },
            { "sub", "user-id-123" }
        };

        // Act
        var userInfo = new UserInfo { Claims = claims };

        // Assert
        using (new AssertionScope())
        {
            userInfo.Claims.Should().HaveCount(2);
            userInfo.Claims["role"].Should().Be("Admin");
            userInfo.Claims["sub"].Should().Be("user-id-123");
        }
    }
}
