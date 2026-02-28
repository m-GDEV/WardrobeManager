using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Endpoints;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Tests.Endpoints;

public class ClothingEndpointsTests
{
    private Mock<IClothingService> _mockClothingService;

    [SetUp]
    public void Setup()
    {
        _mockClothingService = new Mock<IClothingService>();
    }

    #region GetClothing

    [Test]
    public async Task GetClothing_WhenUserHasClothingItems_ReturnsOkWithItems()
    {
        // Arrange
        var user = new User { Id = "test-user-id" };
        var clothingItemDtos = new List<ClothingItemDTO>
        {
            new ClothingItemDTO { Id = 1, Name = "T-Shirt", Category = ClothingCategory.TShirt }
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Items["user"] = user;

        _mockClothingService
            .Setup(s => s.GetAllClothingAsync(user.Id))
            .ReturnsAsync(clothingItemDtos);

        // Act
        var result = await ClothingEndpoints.GetClothing(httpContext, _mockClothingService.Object);

        // Assert
        var okResult = result as Ok<List<ClothingItemDTO>?>;
        using (new AssertionScope())
        {
            okResult.Should().NotBeNull();
            okResult!.Value.Should().HaveCount(1);
        }
    }

    [Test]
    public async Task GetClothing_WhenUserHasNoClothingItems_ReturnsOkWithEmptyList()
    {
        // Arrange
        var user = new User { Id = "empty-user-id" };
        var httpContext = new DefaultHttpContext();
        httpContext.Items["user"] = user;

        _mockClothingService
            .Setup(s => s.GetAllClothingAsync(user.Id))
            .ReturnsAsync(new List<ClothingItemDTO>());

        // Act
        var result = await ClothingEndpoints.GetClothing(httpContext, _mockClothingService.Object);

        // Assert
        var okResult = result as Ok<List<ClothingItemDTO>?>;
        using (new AssertionScope())
        {
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEmpty();
        }
    }

    #endregion

    #region AddNewClothingItem

    [Test]
    public async Task AddNewClothingItem_WhenItemIsValid_ReturnsOk()
    {
        // Arrange
        var user = new User { Id = "test-user-id" };
        var newItem = new NewClothingItemDTO { Name = "Jeans", Category = ClothingCategory.Jeans };

        var httpContext = new DefaultHttpContext();
        httpContext.Items["user"] = user;

        _mockClothingService
            .Setup(s => s.AddNewClothingItem(user.Id, newItem))
            .Returns(Task.CompletedTask);

        // Act
        var result = await ClothingEndpoints.AddNewClothingItem(newItem, httpContext, _mockClothingService.Object, null!);

        // Assert
        result.Should().BeOfType<Ok>();
    }

    [Test]
    public async Task AddNewClothingItem_WhenItemNameIsEmpty_ReturnsBadRequest()
    {
        // Arrange
        var user = new User { Id = "test-user-id" };
        var newItem = new NewClothingItemDTO { Name = "", Category = ClothingCategory.Jeans };

        var httpContext = new DefaultHttpContext();
        httpContext.Items["user"] = user;

        // Act
        var result = await ClothingEndpoints.AddNewClothingItem(newItem, httpContext, _mockClothingService.Object, null!);

        // Assert
        result.Should().BeOfType<BadRequest<string>>();
    }

    #endregion

    #region DeleteClothingItem

    [Test]
    public async Task DeleteClothingItem_WhenCalled_ReturnsOk()
    {
        // Arrange
        var user = new User { Id = "test-user-id" };
        var itemId = 42;

        var httpContext = new DefaultHttpContext();
        httpContext.Items["user"] = user;

        _mockClothingService
            .Setup(s => s.DeleteClothingItem(user.Id, itemId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await ClothingEndpoints.DeleteClothingItem(itemId, httpContext, _mockClothingService.Object, null!);

        // Assert
        _mockClothingService.Verify(s => s.DeleteClothingItem(user.Id, itemId), Times.Once);
        result.Should().BeOfType<Ok>();
    }

    #endregion
}
