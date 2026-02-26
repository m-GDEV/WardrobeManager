using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using WardrobeManager.Api.Endpoints;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Tests.Endpoints;

public class IdentityEndpointsTests
{
    private Mock<IUserService> _mockUserService;

    [SetUp]
    public void Setup()
    {
        _mockUserService = new Mock<IUserService>();
    }

    #region DoesAdminUserExist

    [Test]
    public async Task DoesAdminUserExist_WhenAdminExists_ReturnsTrueResult()
    {
        // Arrange
        _mockUserService.Setup(s => s.DoesAdminUserExist()).ReturnsAsync(true);

        // Act
        var result = await IdentityEndpoints.DoesAdminUserExist(_mockUserService.Object);

        // Assert
        var okResult = result as Ok<bool>;
        using (new AssertionScope())
        {
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeTrue();
        }
    }

    [Test]
    public async Task DoesAdminUserExist_WhenAdminDoesNotExist_ReturnsFalseResult()
    {
        // Arrange
        _mockUserService.Setup(s => s.DoesAdminUserExist()).ReturnsAsync(false);

        // Act
        var result = await IdentityEndpoints.DoesAdminUserExist(_mockUserService.Object);

        // Assert
        var okResult = result as Ok<bool>;
        using (new AssertionScope())
        {
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeFalse();
        }
    }

    #endregion

    #region CreateAdminIfMissing

    [Test]
    public async Task CreateAdminIfMissing_WhenCreatedSuccessfully_ReturnsCreated()
    {
        // Arrange
        var credentials = new AdminUserCredentials { email = "admin@test.com", password = "SecurePass1!" };
        _mockUserService
            .Setup(s => s.CreateAdminIfMissing("admin@test.com", "SecurePass1!"))
            .ReturnsAsync((true, "Admin user created!"));

        // Act
        var result = await IdentityEndpoints.CreateAdminIfMissing(_mockUserService.Object, credentials);

        // Assert
        result.Should().BeOfType<Created>();
    }

    [Test]
    public async Task CreateAdminIfMissing_WhenAdminAlreadyExists_ReturnsConflict()
    {
        // Arrange
        var credentials = new AdminUserCredentials { email = "admin@test.com", password = "SecurePass1!" };
        _mockUserService
            .Setup(s => s.CreateAdminIfMissing("admin@test.com", "SecurePass1!"))
            .ReturnsAsync((false, "Admin user already exists!"));

        // Act
        var result = await IdentityEndpoints.CreateAdminIfMissing(_mockUserService.Object, credentials);

        // Assert
        result.Should().BeOfType<Conflict<string>>();
    }

    #endregion
}
