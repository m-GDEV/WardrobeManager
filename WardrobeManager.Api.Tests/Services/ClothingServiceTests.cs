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
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.StaticResources;

namespace WardrobeManager.Api.Tests.Services;

public class ClothingServiceTests
{
    private const string DefaultUserId = "test-userid";
    private const int DefaultItemId = 1;
    private const string ValidBase64 = "YQ=="; // a

    private Mock<IClothingRepository> _mockRepo;
    private Mock<IMapper> _mockMapper;
    private FakeLogger<ClothingService> _fakeLogger;
    private Mock<IFileService> _mockFileService;
    private Mock<IMiscMethods> _mockMiscMethods;
    private ClothingService _serviceToTest;

    [SetUp]
    public void Setup()
    {
        // Mock the service and its dependencies
        _mockRepo = new Mock<IClothingRepository>();
        _mockMapper = new Mock<IMapper>();
        _fakeLogger = new FakeLogger<ClothingService>();
        _mockFileService = new Mock<IFileService>();
        _mockMiscMethods = new Mock<IMiscMethods>();

        _serviceToTest = new ClothingService(_mockRepo.Object, _mockMapper.Object, _mockFileService.Object,
            _fakeLogger, _mockMiscMethods.Object);
    }

    #region GetClothingItem

    [Test]
    public async Task GetClothingItem_ClothingItemExists_ReturnsClothingItem()
    {
        // Arrange
        var clothingItem = CreateSampleClothingItem();
        var clothingItemDto = CreateSampleClothingItemDTO();
        _mockRepo
            .Setup(r => r.GetAsync(clothingItem.UserId, clothingItem.Id))
            .ReturnsAsync(clothingItem);
        _mockMapper
            .Setup(m => m.Map<ClothingItemDTO>(clothingItem))
            .Returns(clothingItemDto);

// Act
        var result = await _serviceToTest.GetClothingItemAsync(clothingItem.UserId, clothingItem.Id);

// Assert
        result.Should().BeEquivalentTo(clothingItemDto);
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
        var items = CreateSampleClothingItems(5);
        var itemsDto = CreateSampleClothingItemsDTO(5);

        _mockRepo
            .Setup(r => r.GetAllAsync(DefaultUserId))
            .ReturnsAsync(items);

        _mockMapper
            .Setup(m => m.Map<List<ClothingItemDTO>>(items))
            .Returns(itemsDto);
        // Act
        var result = await _serviceToTest.GetAllClothingAsync(items.First().UserId);
        // Assert
        _mockRepo.Verify(r => r.GetAllAsync(items.First().UserId), Times.Once);
        result.Should().BeEquivalentTo(itemsDto);
    }

    [Test]
    public async Task GetAllClothingAsync_MultipleItemsExists_ReturnsAllItems()
    {
        // Arrange
        var items = CreateSampleClothingItems(5);
        var itemsDto = CreateSampleClothingItemsDTO(5);

        _mockRepo
            .Setup(r => r.GetAllAsync(DefaultUserId))
            .ReturnsAsync(items);

        _mockMapper
            .Setup(m => m.Map<List<ClothingItemDTO>>(items))
            .Returns(itemsDto);
        // Act
        var result = await _serviceToTest.GetAllClothingAsync(DefaultUserId);

        // Assert
        result.Should().BeEquivalentTo(itemsDto);
    }

    [Test]
    public async Task GetAllClothingAsync_NoClothingItemsExist_ReturnsEmptyList()
    {
        // Arrange
        var items = new List<ClothingItem>();
        var itemsDto = new List<ClothingItemDTO>();

        _mockRepo.Setup(r => r.GetAllAsync(DefaultUserId)).ReturnsAsync(items);
        _mockMapper.Setup(m => m.Map<List<ClothingItemDTO>>(items)).Returns(itemsDto);

        // Act
        var result = await _serviceToTest.GetAllClothingAsync(DefaultUserId);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(itemsDto);
            result.Should().BeEmpty();
        }
    }

    #endregion

    #region AddNewClothingItem

    [Test]
    public async Task AddNewClothingItem_InvalidImageBase64_DoesNotCreateImageFile()
    {
        // Arrange
        var clothingItem = CreateSampleClothingItem();
        var newClothingItem = CreateSampleNewClothingItem();
        _mockMapper.Setup(m => m.Map<ClothingItem>(newClothingItem)).Returns(clothingItem);
        _mockMiscMethods
            .Setup(x => x.IsValidBase64(newClothingItem.ImageBase64))
            .Returns(false);
        // Act
        await _serviceToTest.AddNewClothingItem(clothingItem.UserId, newClothingItem);
        // Assert
        _mockMapper.Verify(x => x.Map<ClothingItem>(newClothingItem), Times.Once);
        _mockMiscMethods.Verify(x => x.IsValidBase64(newClothingItem.ImageBase64), Times.Once);
        _mockFileService.Verify(
            x => x.SaveImage(It.Is<Guid>(g => g != Guid.Empty), It.IsAny<string>()),
            Times.Never);
        _mockRepo.Verify(x => x.CreateAsync(It.Is<ClothingItem>(ci =>
            ci.UserId == clothingItem.UserId &&
            ci.ImageGuid == null
        )), Times.Once);
        _mockRepo.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Test]
    public async Task AddNewClothingItem_ValidImageBase64_CreatesImageFile()
    {
        // Arrange
        var clothingItem = CreateSampleClothingItem();
        var newClothingItem = CreateSampleNewClothingItem(ValidBase64);
        _mockMapper.Setup(m => m.Map<ClothingItem>(newClothingItem)).Returns(clothingItem);
        _mockMiscMethods
            .Setup(x => x.IsValidBase64(newClothingItem.ImageBase64))
            .Returns(true);
        // Act
        await _serviceToTest.AddNewClothingItem(clothingItem.UserId, newClothingItem);
        // Assert
        _mockMapper.Verify(x => x.Map<ClothingItem>(newClothingItem), Times.Once);
        _mockMiscMethods.Verify(x => x.IsValidBase64(newClothingItem.ImageBase64), Times.Once);
        _mockFileService.Verify(
            x => x.SaveImage(It.Is<Guid>(g => g != Guid.Empty), It.Is<string>(s => s == ValidBase64)),
            Times.Once);
        _mockRepo.Verify(x => x.CreateAsync(It.Is<ClothingItem>(ci =>
            ci.UserId == clothingItem.UserId &&
            ci.ImageGuid != null
        )), Times.Once);
        _mockRepo.Verify(x => x.SaveAsync(), Times.Once);
    }

    #endregion

    #region RemoveClothingItem

    [Test]
    public async Task RemoveClothingItem_ItemDoesNotExist_ErrorLogged()
    {
        // Arrange
        var clothingItem = CreateSampleClothingItem(DefaultUserId, DefaultItemId);
        _mockRepo
            .Setup(r => r.GetAsync(clothingItem.UserId, clothingItem.Id))
            .ReturnsAsync(null as ClothingItem);
        // Act
        await _serviceToTest.DeleteClothingItem(clothingItem.UserId, clothingItem.Id);
        // Assert
        var latestLog = _fakeLogger.Collector.LatestRecord;
        latestLog.Should().NotBeNull();
        latestLog.Level.Should().Be(LogLevel.Information);
        latestLog.Message.Should().Contain("not found"); // or whatever your message is
    }

    [Test]
    public async Task RemoveClothingItem_ItemExistsAndImageGuidNull_FileNotDeleted()
    {
        // Arrange
        var clothingItem = CreateSampleClothingItem(DefaultUserId, DefaultItemId);
        _mockRepo
            .Setup(r => r.GetAsync(DefaultUserId, DefaultItemId))
            .ReturnsAsync(clothingItem);
        // Act
        await _serviceToTest.DeleteClothingItem(DefaultUserId, DefaultItemId);
        // Assert
        _mockRepo.Verify(x => x.Remove(clothingItem), Times.Once);
        _mockRepo.Verify(x => x.SaveAsync(), Times.Once);
        _mockFileService.Verify(x => x.DeleteImage(It.Is<Guid>(s => s == clothingItem.ImageGuid)), Times.Never);
    }

    [Test]
    public async Task RemoveClothingItem_ItemExistsAndImageGuidExists_FileDeleted()
    {
        // Arrange
        var clothingItem = CreateSampleClothingItem(DefaultUserId, DefaultItemId);
        clothingItem.ImageGuid = Guid.NewGuid();
        _mockRepo
            .Setup(r => r.GetAsync(DefaultUserId, DefaultItemId))
            .ReturnsAsync(clothingItem);
        // Act
        await _serviceToTest.DeleteClothingItem(DefaultUserId, DefaultItemId);
        // Assert
        _mockRepo.Verify(x => x.Remove(clothingItem), Times.Once);
        _mockRepo.Verify(x => x.SaveAsync(), Times.Once);
        _mockFileService.Verify(x => x.DeleteImage(It.Is<Guid>(s => s == clothingItem.ImageGuid)), Times.Once);
    }

    #endregion

    #region Private methods

    private ClothingItem CreateSampleClothingItem(Guid? imageGuid = null)
    {
        var clothingItem = new ClothingItem
        {
            Name = "test",
            Category = ClothingCategory.None,
            Season = Season.None,
            Size = ClothingSize.NotSpecified,
            WearLocation = WearLocation.None,
            ImageGuid = imageGuid,
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

    private ClothingItemDTO CreateSampleClothingItemDTO(Guid? imageGuid = null)
    {
        var clothingItem = new ClothingItemDTO
        {
            Name = "test",
            Category = ClothingCategory.None,
            Size = ClothingSize.NotSpecified,
            ImageGuid = imageGuid,
            Id = DefaultItemId
        };
        return clothingItem;
    }

    private ClothingItemDTO CreateSampleClothingItemDTO(int itemId)
    {
        var item = CreateSampleClothingItemDTO();
        item.Id = itemId;
        return item;
    }

    private List<ClothingItemDTO> CreateSampleClothingItemsDTO(int amount, string userId = DefaultUserId)
    {
        List<ClothingItemDTO> items = [];
        var currentItemId = DefaultItemId;
        for (int i = 0; i < amount; i++)
        {
            items.Add(CreateSampleClothingItemDTO(currentItemId++));
        }

        return items;
    }


    private NewClothingItemDTO CreateSampleNewClothingItem(string? imageBase64 = null)
    {
        var item = new NewClothingItemDTO
        {
            Name = "test",
            Category = ClothingCategory.None,
            ImageBase64 = imageBase64,
            Size = ClothingSize.NotSpecified
        };
        return item;
    }

    private List<NewClothingItemDTO> CreateSampleNewClothingItems(int amount)
    {
        List<NewClothingItemDTO> items = [];
        for (int i = 0; i < amount; i++)
        {
            items.Add(CreateSampleNewClothingItem());
        }

        return items;
    }

    #endregion
}