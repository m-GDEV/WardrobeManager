using AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Repositories;
using WardrobeManager.Api.Repositories.Interfaces;
using WardrobeManager.Api.Services.Implementation;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Tests.Services;

public class ClothingServiceTests
{
    private const string DefaultUserId = "test-userid";
    private const int DefaultItemId = 1;

    private Mock<IClothingRepository> _mockRepo;
    private Mock<IMapper> _mockMapper;
    private ClothingService _serviceToTest;

    [SetUp]
    public void Setup()
    {
        // Mock the service and its dependencies
        _mockRepo = new Mock<IClothingRepository>();
        _mockMapper = new Mock<IMapper>();
        _serviceToTest = new ClothingService(_mockRepo.Object, _mockMapper.Object);
    }

    #region GetClothingItem

    [Test]
    public async Task GetClothingItem_ClothingItemExists_ReturnsClothingItem()
    {
        // Arrange
        var clothingItem = CreateSampleClothingItem();
        _mockRepo
            .Setup(r => r.GetAsync(clothingItem.UserId, clothingItem.Id))
            .ReturnsAsync(clothingItem);
        // Act
        var result = await _serviceToTest.GetClothingItemAsync(clothingItem.UserId, clothingItem.Id);
        // Assert
        _mockRepo.Verify(r => r.GetAsync(clothingItem.UserId, clothingItem.Id), Times.Once);
        result.Should().BeEquivalentTo(clothingItem);
    }

    [Test]
    public async Task GetClothingItem_ClothingItemDoesNotExist_ReturnsNull()
    {
        // Arrange
        var clothingItem = CreateSampleClothingItem();
        _mockRepo
            .Setup(r => r.GetAsync(clothingItem.UserId, 1000))
            .ReturnsAsync((ClothingItem?)null);
        // Act
        var result = await _serviceToTest.GetClothingItemAsync(clothingItem.UserId, 1000);
        // Assert
        _mockRepo.Verify(r => r.GetAsync(clothingItem.UserId, 1000), Times.Once);
        result.Should().BeNull();
    }

    #endregion

    #region GetAllClothingAsync

    [Test]
    public async Task GetAllClothingAsync_OneItemExists_ReturnsOneItem()
    {
        // Arrange
        var items = CreateSampleClothingItems(1);
        _mockRepo
            .Setup(r => r.GetAllAsync(items.First().UserId))
            .ReturnsAsync(items);
        // Act
        var result = await _serviceToTest.GetAllClothingAsync(items.First().UserId);
        // Assert
        _mockRepo.Verify(r => r.GetAllAsync(items.First().UserId), Times.Once);
        result.Should().BeEquivalentTo(items);
    }

    [Test]
    public async Task GetAllClothingAsync_MultipleItemsExists_ReturnsAllItems()
    {
        // Arrange
        var items = CreateSampleClothingItems(5);
        _mockRepo
            .Setup(r => r.GetAllAsync(items.First().UserId))
            .ReturnsAsync(items);
        // Act
        var result = await _serviceToTest.GetAllClothingAsync(items.First().UserId);
        // Assert
        _mockRepo.Verify(r => r.GetAllAsync(items.First().UserId), Times.Once);
        result.Should().BeEquivalentTo(items);
    }

    [Test]
    public async Task GetAllClothingAsync_NoClothingItemsExist_ReturnsEmptyList()
    {
        // Arrange
        var items = new List<ClothingItem>();
        _mockRepo
            .Setup(r => r.GetAllAsync(DefaultUserId))
            .ReturnsAsync(items);
        // Act
        var result = await _serviceToTest.GetAllClothingAsync(DefaultUserId);
        // Assert
        _mockRepo.Verify(r => r.GetAllAsync(DefaultUserId), Times.Once);

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(items);
            result.Should().BeEmpty();
        }
    }

    #endregion

    #region Private methods

    private ClothingItem CreateSampleClothingItem()
    {
        var clothingItem = new ClothingItem("T-shirt", ClothingCategory.TShirt, Season.None,
            WearLocation.HomeAndOutside, false, 0, Guid.NewGuid())
        {
            UserId = DefaultUserId,
            Id = DefaultItemId
        };

        return clothingItem;
    }

    private ClothingItem CreateSampleClothingItem(string userId, int itemId)
    {
        var item = CreateSampleClothingItem();
        item.UserId = userId;
        item.Id = itemId;
        return item;
    }

    private List<ClothingItem> CreateSampleClothingItems(int amount, string userId = DefaultUserId)
    {
        List<ClothingItem> items = [];
        var currentItemId = DefaultItemId;
        for (int i = 0; i < amount; i++)
        {
            items.Add(CreateSampleClothingItem(userId, currentItemId++));
        }

        return items;
    }

    #endregion
}