using FluentAssertions;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Shared.Tests.Models;

public class AuthenticationCredentialsModelTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void AuthenticationCredentialsModel_InitializeWithProperties_HasPropertiesSet()
    {
        // Arrange
        var email = "test@test.com";
        var password = "password";
        // Act
        var model = new AuthenticationCredentialsModel
        {
            Email = email,
            Password = password
        };
        // Assert
        model.Email.Should().Be(email);
        model.Password.Should().Be(password);
    }
    
    [Test]
    public void AuthenticationCredentialsModel_DoNotInitialize_HasEmptyStrings()
    {
        // Arrange
        // Act
        var model = new AuthenticationCredentialsModel();
        // Assert
        model.Email.Should().Be(string.Empty);
        model.Password.Should().Be(string.Empty);
    }
}