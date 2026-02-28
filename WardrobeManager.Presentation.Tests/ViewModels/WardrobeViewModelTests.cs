using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Presentation.ViewModels;
using WardrobeManager.Presentation.Tests.Helpers;
using WardrobeManager.Shared.DTOs;
using Blazing.Mvvm.Components;

namespace WardrobeManager.Presentation.Tests.ViewModels;

public class WardrobeViewModelTests
{
    private Mock<IApiService> _mockApiService;
    private Mock<INotificationService> _mockNotificationService;
    private WardrobeViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        _mockApiService = new Mock<IApiService>();
        _mockNotificationService = new Mock<INotificationService>();

        _viewModel = new WardrobeViewModel(
            _mockApiService.Object,
            _mockNotificationService.Object
        );
    }

    [TearDown]
    public void TearDown()
    {
        _viewModel.Dispose();
    }

    #region FetchItemAndUpdate / OnInitializedAsync

    [Test]
    public async Task OnInitializedAsync_WhenCalled_FetchesAndSetsClothingItems()
    {
        // Arrange
        var items = new List<ClothingItemDTO>
        {
            new ClothingItemDTO { Id = 1, Name = "T-Shirt" },
            new ClothingItemDTO { Id = 2, Name = "Jeans" }
        };
        _mockApiService.Setup(s => s.GetAllClothingItemsAsync()).ReturnsAsync(items);

        // Act
        await _viewModel.OnInitializedAsync();

        // Assert
        using (new AssertionScope())
        {
            _viewModel.ClothingItems.Should().HaveCount(2);
            _viewModel.ActionDialogStates.Should().HaveCount(2);
            _viewModel.ActionDialogStates.Keys.Should().Contain(1);
            _viewModel.ActionDialogStates.Keys.Should().Contain(2);
        }
    }

    [Test]
    public async Task FetchItemAndUpdate_WhenCalled_ResetsDialogStates()
    {
        // Arrange - first fetch sets up states
        var items = new List<ClothingItemDTO> { new ClothingItemDTO { Id = 1, Name = "T-Shirt" } };
        _mockApiService.Setup(s => s.GetAllClothingItemsAsync()).ReturnsAsync(items);
        await _viewModel.FetchItemAndUpdate();

        // Manually set a dialog state
        _viewModel.ActionDialogStates[1].ShowDelete = true;

        // Act - second fetch should reset
        await _viewModel.FetchItemAndUpdate();

        // Assert - state was reset
        _viewModel.ActionDialogStates[1].ShowDelete.Should().BeFalse();
    }

    #endregion

    #region RemoveItem

    [Test]
    public async Task RemoveItem_WhenCalled_DeletesItemAndRefreshes()
    {
        // Arrange
        var itemId = 42;
        var items = new List<ClothingItemDTO> { new ClothingItemDTO { Id = 99, Name = "Remaining" } };
        _mockApiService.Setup(s => s.DeleteClothingItemAsync(itemId)).Returns(Task.CompletedTask);
        _mockApiService.Setup(s => s.GetAllClothingItemsAsync()).ReturnsAsync(items);

        // Act
        await _viewModel.RemoveItem(itemId);

        // Assert
        _mockApiService.Verify(s => s.DeleteClothingItemAsync(itemId), Times.Once);
        _mockApiService.Verify(s => s.GetAllClothingItemsAsync(), Times.Once);
        _viewModel.ClothingItems.Should().HaveCount(1);
    }

    #endregion

    #region UpdateActionDialogState

    [Test]
    public async Task UpdateActionDialogState_Delete_SetsShowDeleteFlag()
    {
        // Arrange
        var items = new List<ClothingItemDTO> { new ClothingItemDTO { Id = 1, Name = "T-Shirt" } };
        _mockApiService.Setup(s => s.GetAllClothingItemsAsync()).ReturnsAsync(items);
        await _viewModel.FetchItemAndUpdate();

        // Act
        _viewModel.UpdateActionDialogState(1, ActionType.Delete, true);

        // Assert
        _viewModel.ActionDialogStates[1].ShowDelete.Should().BeTrue();
    }

    [Test]
    public async Task UpdateActionDialogState_Edit_SetsShowEditFlag()
    {
        // Arrange
        var items = new List<ClothingItemDTO> { new ClothingItemDTO { Id = 1, Name = "T-Shirt" } };
        _mockApiService.Setup(s => s.GetAllClothingItemsAsync()).ReturnsAsync(items);
        await _viewModel.FetchItemAndUpdate();

        // Act
        _viewModel.UpdateActionDialogState(1, ActionType.Edit, true);

        // Assert
        _viewModel.ActionDialogStates[1].ShowEdit.Should().BeTrue();
    }

    [Test]
    public void UpdateActionDialogState_WhenItemDoesNotExist_NotifiesError()
    {
        // Arrange - ActionDialogStates is empty

        // Act
        _viewModel.UpdateActionDialogState(999, ActionType.Delete, true);

        // Assert
        _mockNotificationService.Verify(
            s => s.AddNotification(It.IsAny<string>(), WardrobeManager.Shared.Enums.NotificationType.Error),
            Times.Once);
    }

    #endregion

    #region GetActionStateSafely

    [Test]
    public async Task GetActionStateSafely_WhenDeleteIsSet_ReturnsTrue()
    {
        // Arrange
        var items = new List<ClothingItemDTO> { new ClothingItemDTO { Id = 1, Name = "T-Shirt" } };
        _mockApiService.Setup(s => s.GetAllClothingItemsAsync()).ReturnsAsync(items);
        await _viewModel.FetchItemAndUpdate();
        _viewModel.UpdateActionDialogState(1, ActionType.Delete, true);

        // Act
        var result = _viewModel.GetActionStateSafely(1, ActionType.Delete);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void GetActionStateSafely_WhenItemDoesNotExist_ReturnsFalse()
    {
        // Arrange - no items loaded

        // Act
        var result = _viewModel.GetActionStateSafely(999, ActionType.Delete);

        // Assert
        result.Should().BeFalse();
    }

    #endregion
}
