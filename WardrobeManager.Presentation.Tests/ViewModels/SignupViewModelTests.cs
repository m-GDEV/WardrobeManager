using Blazing.Mvvm.Components;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Identity.Models;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Presentation.ViewModels;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Presentation.Tests.ViewModels;

public class SignupViewModelTests
{
    private Mock<INotificationService> _mockNotificationService;
    private Mock<IMvvmNavigationManager> _mockNavManager;
    private Mock<IIdentityService> _mockIdentityService;
    private SignupViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        _mockNotificationService = new Mock<INotificationService>();
        _mockNavManager = new Mock<IMvvmNavigationManager>();
        _mockIdentityService = new Mock<IIdentityService>();

        _viewModel = new SignupViewModel(
            _mockNotificationService.Object,
            _mockNavManager.Object,
            _mockIdentityService.Object
        );
    }

    [TearDown]
    public void TearDown()
    {
        _viewModel.Dispose();
    }

    #region SetEmail / SetPassword

    [Test]
    public void SetEmail_WhenCalled_SetsEmailOnCredentialsModel()
    {
        // Arrange
        const string email = "signup@test.com";

        // Act
        _viewModel.SetEmail(email);

        // Assert
        _viewModel.AuthenticationCredentialsModel.Email.Should().Be(email);
    }

    [Test]
    public void SetPassword_WhenCalled_SetsPasswordOnCredentialsModel()
    {
        // Arrange
        const string password = "mypassword";

        // Act
        _viewModel.SetPassword(password);

        // Assert
        _viewModel.AuthenticationCredentialsModel.Password.Should().Be(password);
    }

    #endregion

    #region SignupAsync

    [Test]
    public async Task SignupAsync_WhenBothSucceed_NavigatesToDashboard()
    {
        // Arrange
        _mockIdentityService
            .Setup(s => s.SignupAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(true);
        _mockIdentityService
            .Setup(s => s.LoginAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(true);

        // Act
        await _viewModel.SignupAsync();

        // Assert
        _mockNavManager.Verify(n => n.NavigateTo<DashboardViewModel>(false, false), Times.Once);
    }

    [Test]
    public async Task SignupAsync_WhenSignupFails_DoesNotAttemptLogin()
    {
        // Arrange
        _mockIdentityService
            .Setup(s => s.SignupAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(false);

        // Act
        await _viewModel.SignupAsync();

        // Assert
        _mockIdentityService.Verify(s => s.LoginAsync(It.IsAny<AuthenticationCredentialsModel>()), Times.Never);
    }

    [Test]
    public async Task SignupAsync_WhenSignupSucceedsButLoginFails_AddsErrorNotification()
    {
        // Arrange
        _mockIdentityService
            .Setup(s => s.SignupAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(true);
        _mockIdentityService
            .Setup(s => s.LoginAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(false);

        // Act
        await _viewModel.SignupAsync();

        // Assert
        _mockNotificationService.Verify(n =>
            n.AddNotification(It.IsAny<string>(), NotificationType.Error), Times.Once);
    }

    [Test]
    public async Task SignupAsync_WhenBothSucceed_AddsSuccessNotification()
    {
        // Arrange
        _mockIdentityService
            .Setup(s => s.SignupAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(true);
        _mockIdentityService
            .Setup(s => s.LoginAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(true);

        // Act
        await _viewModel.SignupAsync();

        // Assert
        _mockNotificationService.Verify(n =>
            n.AddNotification(It.IsAny<string>(), NotificationType.Success), Times.Once);
    }

    #endregion

    #region DetectEnterPressed

    [Test]
    public async Task DetectEnterPressed_WhenEnterKey_CallsSignupAsync()
    {
        // Arrange
        _mockIdentityService
            .Setup(s => s.SignupAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(true);
        _mockIdentityService
            .Setup(s => s.LoginAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(true);
        var eventArgs = new KeyboardEventArgs { Key = "Enter" };

        // Act
        await _viewModel.DetectEnterPressed(eventArgs);

        // Assert
        _mockIdentityService.Verify(s => s.SignupAsync(It.IsAny<AuthenticationCredentialsModel>()), Times.Once);
    }

    [Test]
    public async Task DetectEnterPressed_WhenOtherKey_DoesNotCallSignupAsync()
    {
        // Arrange
        var eventArgs = new KeyboardEventArgs { Key = "Tab" };

        // Act
        await _viewModel.DetectEnterPressed(eventArgs);

        // Assert
        _mockIdentityService.Verify(s => s.SignupAsync(It.IsAny<AuthenticationCredentialsModel>()), Times.Never);
    }

    #endregion
}
