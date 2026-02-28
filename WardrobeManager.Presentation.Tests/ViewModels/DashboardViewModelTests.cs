using FluentAssertions;
using WardrobeManager.Presentation.ViewModels;

namespace WardrobeManager.Presentation.Tests.ViewModels;

public class DashboardViewModelTests
{
    private DashboardViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        _viewModel = new DashboardViewModel();
    }

    [TearDown]
    public void TearDown()
    {
        _viewModel.Dispose();
    }

    [Test]
    public void DashboardViewModel_WhenInstantiated_IsNotNull()
    {
        // Assert
        _viewModel.Should().NotBeNull();
    }
}
