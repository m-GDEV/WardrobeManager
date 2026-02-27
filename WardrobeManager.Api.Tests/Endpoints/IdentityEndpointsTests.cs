using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using WardrobeManager.Api.Database.Entities;
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

    #region LogoutAsync

    [Test]
    public async Task LogoutAsync_WhenBodyIsNotNull_CallsSignOutAndReturnsOk()
    {
        // Arrange
        var userStore = new Mock<IUserStore<User>>();
        var mockSignInManager = new Mock<SignInManager<User>>(
            new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null).Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            null, null, null, null);

        // Act
        var result = await IdentityEndpoints.LogoutAsync(mockSignInManager.Object, new object());

        // Assert
        mockSignInManager.Verify(s => s.SignOutAsync(), Times.Once);
        result.Should().BeOfType<Ok>();
    }

    [Test]
    public async Task LogoutAsync_WhenBodyIsNull_ReturnsUnauthorized()
    {
        // Arrange
        var userStore = new Mock<IUserStore<User>>();
        var mockSignInManager = new Mock<SignInManager<User>>(
            new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null).Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            null, null, null, null);

        // Act
        var result = await IdentityEndpoints.LogoutAsync(mockSignInManager.Object, null!);

        // Assert
        mockSignInManager.Verify(s => s.SignOutAsync(), Times.Never);
        result.Should().BeOfType<UnauthorizedHttpResult>();
    }

    #endregion

    #region RolesAsync

    [Test]
    public async Task RolesAsync_WhenUserIsAuthenticated_ReturnsJsonWithRoles()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "test@test.com"),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var identity = new ClaimsIdentity(claims, "TestScheme");
        var principal = new ClaimsPrincipal(identity);

        // Act
        var result = await IdentityEndpoints.RolesAsync(principal);

        // Assert
        result.Should().NotBeOfType<UnauthorizedHttpResult>();
    }

    [Test]
    public async Task RolesAsync_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity()); // anonymous

        // Act
        var result = await IdentityEndpoints.RolesAsync(principal);

        // Assert
        result.Should().BeOfType<UnauthorizedHttpResult>();
    }

    #endregion
}
