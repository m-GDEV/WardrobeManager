using Blazing.Mvvm.Components;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Presentation.ViewModels;
using WardrobeManager.Presentation.Identity;
using WardrobeManager.Presentation.Identity.Models;
using WardrobeManager.Presentation.Tests.Helpers;
using WardrobeManager.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace WardrobeManager.Presentation.Tests.ViewModels;

public class LoginViewModelTests
{
    private Mock<IMvvmNavigationManager> _mockNavManager;
    private Mock<IIdentityService> _mockIdentityService;
    private LoginViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        _mockNavManager = new Mock<IMvvmNavigationManager>();
        _mockIdentityService = new Mock<IIdentityService>();

        _viewModel = new LoginViewModel(
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
        const string email = "test@test.com";

        // Act
        _viewModel.SetEmail(email);

        // Assert
        _viewModel.AuthenticationCredentialsModel.Email.Should().Be(email);
    }

    [Test]
    public void SetPassword_WhenCalled_SetsPasswordOnCredentialsModel()
    {
        // Arrange
        const string password = "securepassword";

        // Act
        _viewModel.SetPassword(password);

        // Assert
        _viewModel.AuthenticationCredentialsModel.Password.Should().Be(password);
    }

    #endregion

    #region LoginAsync

    [Test]
    public async Task LoginAsync_WhenLoginSucceeds_NavigatesToDashboard()
    {
        // Arrange
        _mockIdentityService
            .Setup(s => s.LoginAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(true);

        // Act
        await _viewModel.LoginAsync();

        // Assert
        _mockNavManager.Verify(n => n.NavigateTo<DashboardViewModel>(false, false), Times.Once);
    }

    [Test]
    public async Task LoginAsync_WhenLoginFails_DoesNotNavigate()
    {
        // Arrange
        _mockIdentityService
            .Setup(s => s.LoginAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(false);

        // Act
        await _viewModel.LoginAsync();

        // Assert
        _mockNavManager.Verify(n => n.NavigateTo<DashboardViewModel>(false, false), Times.Never);
    }

    #endregion

    #region DetectEnterPressed

    [Test]
    public async Task DetectEnterPressed_WhenEnterKey_CallsLoginAsync()
    {
        // Arrange
        _mockIdentityService
            .Setup(s => s.LoginAsync(It.IsAny<AuthenticationCredentialsModel>()))
            .ReturnsAsync(true);
        var eventArgs = new KeyboardEventArgs { Key = "Enter" };

        // Act
        await _viewModel.DetectEnterPressed(eventArgs);

        // Assert
        _mockIdentityService.Verify(s => s.LoginAsync(It.IsAny<AuthenticationCredentialsModel>()), Times.Once);
    }

    [Test]
    public async Task DetectEnterPressed_WhenOtherKey_DoesNotCallLoginAsync()
    {
        // Arrange
        var eventArgs = new KeyboardEventArgs { Key = "a" };

        // Act
        await _viewModel.DetectEnterPressed(eventArgs);

        // Assert
        _mockIdentityService.Verify(s => s.LoginAsync(It.IsAny<AuthenticationCredentialsModel>()), Times.Never);
    }

    #endregion

    #region OnInitializedAsync

    [Test]
    public async Task OnInitializedAsync_WhenAlreadyAuthenticated_NavigatesToDashboard()
    {
        // Arrange
        _mockIdentityService.Setup(s => s.IsAuthenticated()).ReturnsAsync(true);

        // Act
        await _viewModel.OnInitializedAsync();

        // Assert
        _mockNavManager.Verify(n => n.NavigateTo<DashboardViewModel>(false, false), Times.Once);
    }

    [Test]
    public async Task OnInitializedAsync_WhenNotAuthenticated_DoesNotNavigate()
    {
        // Arrange
        _mockIdentityService.Setup(s => s.IsAuthenticated()).ReturnsAsync(false);

        // Act
        await _viewModel.OnInitializedAsync();

        // Assert
        _mockNavManager.Verify(n => n.NavigateTo<DashboardViewModel>(false, false), Times.Never);
    }

    #endregion
}
