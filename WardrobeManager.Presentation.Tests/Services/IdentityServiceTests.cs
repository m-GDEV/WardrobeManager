using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using System.Security.Claims;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Identity.Models;
using WardrobeManager.Presentation.Services.Implementation;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Tests.Services;

public class IdentityServiceTests
{
    private Mock<IAccountManagement> _mockAccountManagement;
    private Mock<INotificationService> _mockNotificationService;
    private IdentityService _service;

    [SetUp]
    public void Setup()
    {
        _mockAccountManagement = new Mock<IAccountManagement>();
        _mockNotificationService = new Mock<INotificationService>();
        _service = new IdentityService(_mockAccountManagement.Object, _mockNotificationService.Object);
    }

    #region SignupAsync

    [Test]
    public async Task SignupAsync_WhenSuccessful_ReturnsTrue()
    {
        // Arrange
        var credentials = new AuthenticationCredentialsModel { Email = "test@test.com", Password = "password" };
        _mockAccountManagement
            .Setup(a => a.RegisterAsync(credentials.Email, credentials.Password))
            .ReturnsAsync(new FormResult { Succeeded = true });

        // Act
        var result = await _service.SignupAsync(credentials);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task SignupAsync_WhenFailed_ReturnsFalse()
    {
        // Arrange
        var credentials = new AuthenticationCredentialsModel { Email = "test@test.com", Password = "password" };
        _mockAccountManagement
            .Setup(a => a.RegisterAsync(credentials.Email, credentials.Password))
            .ReturnsAsync(new FormResult { Succeeded = false, ErrorList = ["Email already taken"] });

        // Act
        var result = await _service.SignupAsync(credentials);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task SignupAsync_WhenFailed_AddsErrorNotifications()
    {
        // Arrange
        var credentials = new AuthenticationCredentialsModel { Email = "test@test.com", Password = "password" };
        _mockAccountManagement
            .Setup(a => a.RegisterAsync(credentials.Email, credentials.Password))
            .ReturnsAsync(new FormResult
            {
                Succeeded = false,
                ErrorList = ["Email already taken", "Password too short"]
            });

        // Act
        await _service.SignupAsync(credentials);

        // Assert
        _mockNotificationService.Verify(n =>
            n.AddNotification(It.IsAny<string>(), NotificationType.Error), Times.Exactly(2));
    }

    #endregion

    #region LoginAsync

    [Test]
    public async Task LoginAsync_WhenSuccessful_ReturnsTrue()
    {
        // Arrange
        var credentials = new AuthenticationCredentialsModel { Email = "test@test.com", Password = "password" };
        _mockAccountManagement
            .Setup(a => a.LoginAsync(credentials.Email, credentials.Password))
            .ReturnsAsync(new FormResult { Succeeded = true });

        // Act
        var result = await _service.LoginAsync(credentials);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task LoginAsync_WhenFailed_ReturnsFalse()
    {
        // Arrange
        var credentials = new AuthenticationCredentialsModel { Email = "test@test.com", Password = "password" };
        _mockAccountManagement
            .Setup(a => a.LoginAsync(credentials.Email, credentials.Password))
            .ReturnsAsync(new FormResult { Succeeded = false, ErrorList = ["Invalid email and/or password."] });

        // Act
        var result = await _service.LoginAsync(credentials);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task LoginAsync_WhenFailed_AddsErrorNotification()
    {
        // Arrange
        var credentials = new AuthenticationCredentialsModel { Email = "test@test.com", Password = "bad" };
        _mockAccountManagement
            .Setup(a => a.LoginAsync(credentials.Email, credentials.Password))
            .ReturnsAsync(new FormResult { Succeeded = false, ErrorList = ["Invalid email and/or password."] });

        // Act
        await _service.LoginAsync(credentials);

        // Assert
        _mockNotificationService.Verify(n =>
            n.AddNotification("Invalid email and/or password.", NotificationType.Error), Times.Once);
    }

    #endregion

    #region LogoutAsync

    [Test]
    public async Task LogoutAsync_WhenSuccessful_ReturnsTrue()
    {
        // Arrange
        _mockAccountManagement.Setup(a => a.LogoutAsync()).ReturnsAsync(true);
        _mockAccountManagement
            .Setup(a => a.GetAuthenticationStateAsync())
            .ReturnsAsync(new AuthenticationState(new ClaimsPrincipal()));

        // Act
        var result = await _service.LogoutAsync();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task LogoutAsync_WhenFailed_ReturnsFalse()
    {
        // Arrange
        _mockAccountManagement.Setup(a => a.LogoutAsync()).ReturnsAsync(false);

        // Act
        var result = await _service.LogoutAsync();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task LogoutAsync_WhenFailed_AddsErrorNotification()
    {
        // Arrange
        _mockAccountManagement.Setup(a => a.LogoutAsync()).ReturnsAsync(false);

        // Act
        await _service.LogoutAsync();

        // Assert
        _mockNotificationService.Verify(n =>
            n.AddNotification(It.IsAny<string>(), NotificationType.Error), Times.Once);
    }

    [Test]
    public async Task LogoutAsync_WhenSuccessful_AddsSuccessNotification()
    {
        // Arrange
        _mockAccountManagement.Setup(a => a.LogoutAsync()).ReturnsAsync(true);
        _mockAccountManagement
            .Setup(a => a.GetAuthenticationStateAsync())
            .ReturnsAsync(new AuthenticationState(new ClaimsPrincipal()));

        // Act
        await _service.LogoutAsync();

        // Assert
        _mockNotificationService.Verify(n =>
            n.AddNotification(It.IsAny<string>(), NotificationType.Success), Times.Once);
    }

    #endregion

    #region IsAuthenticated

    [Test]
    public async Task IsAuthenticated_WhenAuthenticated_ReturnsTrue()
    {
        // Arrange
        var claims = new[] { new Claim(ClaimTypes.Name, "test@test.com") };
        var identity = new ClaimsIdentity(claims, "TestScheme");
        var principal = new ClaimsPrincipal(identity);
        _mockAccountManagement
            .Setup(a => a.GetAuthenticationStateAsync())
            .ReturnsAsync(new AuthenticationState(principal));

        // Act
        var result = await _service.IsAuthenticated();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task IsAuthenticated_WhenNotAuthenticated_ReturnsFalse()
    {
        // Arrange - anonymous principal (no authentication type)
        var principal = new ClaimsPrincipal(new ClaimsIdentity());
        _mockAccountManagement
            .Setup(a => a.GetAuthenticationStateAsync())
            .ReturnsAsync(new AuthenticationState(principal));

        // Act
        var result = await _service.IsAuthenticated();

        // Assert
        result.Should().BeFalse();
    }

    #endregion
}
