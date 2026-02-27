using Blazing.Mvvm.Components;
using FluentAssertions;
using Moq;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Presentation.Tests.Helpers;
using WardrobeManager.Presentation.ViewModels;

namespace WardrobeManager.Presentation.Tests.ViewModels;

public class HomeViewModelTests
{
    private Mock<INotificationService> _mockNotificationService;
    private FakeNavigationManager _fakeNavigationManager;
    private Mock<IMvvmNavigationManager> _mockNavManager;
    private Mock<IIdentityService> _mockIdentityService;
    private HomeViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        _mockNotificationService = new Mock<INotificationService>();
        _fakeNavigationManager = new FakeNavigationManager();
        _mockNavManager = new Mock<IMvvmNavigationManager>();
        _mockIdentityService = new Mock<IIdentityService>();

        _viewModel = new HomeViewModel(
            _mockNotificationService.Object,
            _fakeNavigationManager,
            _mockNavManager.Object,
            _mockIdentityService.Object
        );
    }

    [TearDown]
    public void TearDown()
    {
        _viewModel.Dispose();
    }

    [Test]
    public void HomeViewModel_WhenInstantiated_IsNotNull()
    {
        // Assert
        _viewModel.Should().NotBeNull();
    }
}
