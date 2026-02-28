using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using WardrobeManager.Presentation.Services.Interfaces;
using WardrobeManager.Presentation.ViewModels;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.StaticResources;
using Blazing.Mvvm.Components;
using Microsoft.Extensions.Configuration;

namespace WardrobeManager.Presentation.Tests.ViewModels;

public class AddClothingItemViewModelTests
{
    private Mock<IMvvmNavigationManager> _mockNavManager;
    private Mock<IApiService> _mockApiService;
    private Mock<INotificationService> _mockNotificationService;
    private Mock<IMiscMethods> _mockMiscMethods;
    private Mock<IConfiguration> _mockConfiguration;
    private AddClothingItemViewModel _viewModel;

    [SetUp]
    public void Setup()
    {
        _mockNavManager = new Mock<IMvvmNavigationManager>();
        _mockApiService = new Mock<IApiService>();
        _mockNotificationService = new Mock<INotificationService>();
        _mockMiscMethods = new Mock<IMiscMethods>();
        _mockConfiguration = new Mock<IConfiguration>();

        _mockMiscMethods
            .Setup(m => m.ConvertEnumToCollection<ClothingCategory>())
            .Returns(new List<ClothingCategory> { ClothingCategory.TShirt, ClothingCategory.Jeans });
        _mockMiscMethods
            .Setup(m => m.ConvertEnumToCollection<ClothingSize>())
            .Returns(new List<ClothingSize> { ClothingSize.Small, ClothingSize.Medium });

        _viewModel = new AddClothingItemViewModel(
            _mockNavManager.Object,
            _mockApiService.Object,
            _mockNotificationService.Object,
            _mockMiscMethods.Object,
            _mockConfiguration.Object
        );
    }

    [TearDown]
    public void TearDown()
    {
        _viewModel.Dispose();
    }

    #region SubmitAsync

    [Test]
    public async Task SubmitAsync_WhenItemIsValid_CallsApiAndNotifiesSuccess()
    {
        // Arrange
        _viewModel.NewClothingItem = new NewClothingItemDTO { Name = "My Jeans", Category = ClothingCategory.Jeans };
        _mockApiService.Setup(s => s.AddNewClothingItemAsync(It.IsAny<NewClothingItemDTO>())).Returns(Task.CompletedTask);

        // Act
        await _viewModel.SubmitAsync();

        // Assert
        using (new AssertionScope())
        {
            _mockApiService.Verify(s => s.AddNewClothingItemAsync(It.IsAny<NewClothingItemDTO>()), Times.Once);
            _mockNotificationService.Verify(
                s => s.AddNotification(It.Is<string>(m => m.Contains("My Jeans")), NotificationType.Success),
                Times.Once);
        }
    }

    [Test]
    public async Task SubmitAsync_WhenItemIsValid_ResetsNewClothingItem()
    {
        // Arrange
        _viewModel.NewClothingItem = new NewClothingItemDTO { Name = "My Jeans", Category = ClothingCategory.Jeans };
        _mockApiService.Setup(s => s.AddNewClothingItemAsync(It.IsAny<NewClothingItemDTO>())).Returns(Task.CompletedTask);

        // Act
        await _viewModel.SubmitAsync();

        // Assert - NewClothingItem should be reset to default
        _viewModel.NewClothingItem.Name.Should().Be(string.Empty);
    }

    [Test]
    public async Task SubmitAsync_WhenItemNameIsEmpty_DoesNotCallApi()
    {
        // Arrange
        _viewModel.NewClothingItem = new NewClothingItemDTO { Name = "", Category = ClothingCategory.Jeans };

        // Act
        await _viewModel.SubmitAsync();

        // Assert
        using (new AssertionScope())
        {
            _mockApiService.Verify(s => s.AddNewClothingItemAsync(It.IsAny<NewClothingItemDTO>()), Times.Never);
            _mockNotificationService.Verify(
                s => s.AddNotification(It.IsAny<string>(), NotificationType.Error),
                Times.Once);
        }
    }

    #endregion

    #region GetNameWithSpacesAndEmoji

    [Test]
    public void GetNameWithSpacesAndEmoji_WhenCalled_DelegatesToMiscMethods()
    {
        // Arrange
        _mockMiscMethods
            .Setup(m => m.GetNameWithSpacesFromEnum(ClothingCategory.TShirt))
            .Returns("👕 T Shirt");

        // Act
        var result = _viewModel.GetNameWithSpacesAndEmoji(ClothingCategory.TShirt);

        // Assert
        result.Should().Be("👕 T Shirt");
    }

    #endregion

    #region ClothingCategories / ClothingSizes

    [Test]
    public void ClothingCategories_WhenInstantiated_IsPopulated()
    {
        _viewModel.ClothingCategories.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void ClothingSizes_WhenInstantiated_IsPopulated()
    {
        _viewModel.ClothingSizes.Should().NotBeNullOrEmpty();
    }

    #endregion
}
