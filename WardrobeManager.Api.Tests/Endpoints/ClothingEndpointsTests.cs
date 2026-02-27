using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Endpoints;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Tests.Endpoints;

public class ClothingEndpointsTests
{
    private Mock<IClothingService> _mockClothingService;
    private Mock<DatabaseContext> _mockDbContext;

    [SetUp]
    public void Setup()
    {
        _mockClothingService = new Mock<IClothingService>();
        _mockDbContext = new Mock<DatabaseContext>();
    }

    #region GetClothing

    [Test]
    public async Task GetClothing_WhenUserHasClothingItems_ReturnsOkWithItems()
    {
        // Arrange
        var user = new User { Id = "test-user-id" };
        var clothingItems = new List<ClothingItem>
        {
            new ClothingItem("T-Shirt", ClothingCategory.TShirt, Season.Fall, WearLocation.Outside, false, 3, null)
            {
                UserId = user.Id
            }
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Items["user"] = user;

        _mockClothingService
            .Setup(s => s.GetAllClothingAsync(user.Id))
            .ReturnsAsync(clothingItems);

        // Act
        var result = await ClothingEndpoints.GetClothing(httpContext, _mockClothingService.Object, null!);

        // Assert
        var okResult = result as Ok<List<ClothingItem>?>;
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
            .ReturnsAsync(new List<ClothingItem>());

        // Act
        var result = await ClothingEndpoints.GetClothing(httpContext, _mockClothingService.Object, null!);

        // Assert
        var okResult = result as Ok<List<ClothingItem>?>;
        using (new AssertionScope())
        {
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEmpty();
        }
    }

    #endregion
}
