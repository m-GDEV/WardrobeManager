using Blazing.Mvvm.Components;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System.Security.Claims;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Presentation.ViewModels;
using WardrobeManager.Presentation.Tests.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace WardrobeManager.Presentation.Tests.ViewModels;

public class NavBarViewModelTests
{
    private Mock<INotificationService> _mockNotificationService;
    private Mock<IMvvmNavigationManager> _mockNavManager;
    private Mock<IIdentityService> _mockIdentityService;
    private Mock<IApiService> _mockApiService;
    private NavBarViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        _mockNotificationService = new Mock<INotificationService>();
        _mockNavManager = new Mock<IMvvmNavigationManager>();
        _mockIdentityService = new Mock<IIdentityService>();
        _mockApiService = new Mock<IApiService>();

        _viewModel = new NavBarViewModel(
            _mockNotificationService.Object,
            _mockNavManager.Object,
            _mockIdentityService.Object,
            _mockApiService.Object
        );
    }

    [TearDown]
    public void TearDown()
    {
        _viewModel.Dispose();
    }

    #region ToggleUserPopover

    [Test]
    public void ToggleUserPopover_WhenCalledOnce_ShowsPopover()
    {
        // Arrange - popover initially hidden

        // Act
        _viewModel.ToggleUserPopover();

        // Assert
        _viewModel.ShowUserPopover.Should().BeTrue();
    }

    [Test]
    public void ToggleUserPopover_WhenCalledTwice_HidesPopover()
    {
        // Arrange

        // Act
        _viewModel.ToggleUserPopover();
        _viewModel.ToggleUserPopover();

        // Assert
        _viewModel.ShowUserPopover.Should().BeFalse();
    }

    #endregion

    #region OnInitializedAsync

    [Test]
    public async Task OnInitializedAsync_WhenApiIsUp_SetsCanConnectToBackendTrue()
    {
        // Arrange
        _mockApiService.Setup(s => s.CheckApiConnection()).ReturnsAsync(true);
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Name, "test@test.com") }, "TestScheme"));
        _mockIdentityService.Setup(s => s.GetUserInformation()).ReturnsAsync(principal);

        // Act
        await _viewModel.OnInitializedAsync();

        // Assert
        _viewModel.CanConnectToBackend.Should().BeTrue();
    }

    [Test]
    public async Task OnInitializedAsync_WhenApiIsDown_SetsCanConnectToBackendFalse()
    {
        // Arrange
        _mockApiService.Setup(s => s.CheckApiConnection()).ReturnsAsync(false);
        var principal = new ClaimsPrincipal(new ClaimsIdentity());
        _mockIdentityService.Setup(s => s.GetUserInformation()).ReturnsAsync(principal);

        // Act
        await _viewModel.OnInitializedAsync();

        // Assert
        _viewModel.CanConnectToBackend.Should().BeFalse();
    }

    [Test]
    public async Task OnInitializedAsync_WhenUserHasName_SetsUsersName()
    {
        // Arrange
        _mockApiService.Setup(s => s.CheckApiConnection()).ReturnsAsync(true);
        var claims = new[] { new Claim(ClaimTypes.Name, "John Doe") };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestScheme"));
        _mockIdentityService.Setup(s => s.GetUserInformation()).ReturnsAsync(principal);

        // Act
        await _viewModel.OnInitializedAsync();

        // Assert
        _viewModel.UsersName.Should().Be("John Doe");
    }

    [Test]
    public async Task OnInitializedAsync_WhenUserHasNoName_SetsDefaultName()
    {
        // Arrange
        _mockApiService.Setup(s => s.CheckApiConnection()).ReturnsAsync(true);
        var principal = new ClaimsPrincipal(new ClaimsIdentity()); // anonymous
        _mockIdentityService.Setup(s => s.GetUserInformation()).ReturnsAsync(principal);

        // Act
        await _viewModel.OnInitializedAsync();

        // Assert
        _viewModel.UsersName.Should().Be("Logged In User");
    }

    #endregion

    #region LogoutAsync

    [Test]
    public async Task LogoutAsync_WhenCalled_CallsIdentityServiceLogout()
    {
        // Arrange
        _mockIdentityService.Setup(s => s.LogoutAsync()).ReturnsAsync(true);

        // Act
        await _viewModel.LogoutAsync();

        // Assert
        _mockIdentityService.Verify(s => s.LogoutAsync(), Times.Once);
    }

    [Test]
    public async Task LogoutAsync_WhenCalled_HidesPopoverAndNavigatesHome()
    {
        // Arrange
        _mockIdentityService.Setup(s => s.LogoutAsync()).ReturnsAsync(true);
        _viewModel.ToggleUserPopover(); // Show the popover first

        // Act
        await _viewModel.LogoutAsync();

        // Assert
        using (new AssertionScope())
        {
            _viewModel.ShowUserPopover.Should().BeFalse();
            _mockNavManager.Verify(n => n.NavigateTo<HomeViewModel>(false, false), Times.Once);
        }
    }

    #endregion
}
