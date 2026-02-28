using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Repositories.Implementation;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Tests.Repositories;

public class ClothingRepositoryTests
{
    private DatabaseContext _context;
    private ClothingRepository _repo;
    private const string DefaultUserId = "test-user-id";

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options);

        // ClothingRepository constructor only takes DatabaseContext
        _repo = new ClothingRepository(_context);

        // Seed a user so foreign key constraint is satisfied
        var user = new User { Id = DefaultUserId, UserName = "testuser" };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.DisposeAsync();
    }

    private async Task<ClothingItem> AddSampleItemAsync(string userId = DefaultUserId, string name = "T-Shirt")
    {
        var item = new ClothingItem(name, ClothingCategory.TShirt, Season.Fall,
            WearLocation.HomeAndOutside, false, 3, null)
        {
            UserId = userId
        };
        await _context.ClothingItems.AddAsync(item);
        await _context.SaveChangesAsync();
        return item;
    }

    #region GetAsync(userId, itemId)

    [Test]
    public async Task GetAsync_ByUserIdAndItemId_WhenExists_ReturnsItem()
    {
        // Arrange
        var item = await AddSampleItemAsync();

        // Act
        var result = await _repo.GetAsync(DefaultUserId, item.Id);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result!.Id.Should().Be(item.Id);
            result.UserId.Should().Be(DefaultUserId);
        }
    }

    [Test]
    public async Task GetAsync_ByUserIdAndItemId_WhenItemBelongsToDifferentUser_ReturnsNull()
    {
        // Arrange
        var item = await AddSampleItemAsync("different-user-id");

        // Act
        var result = await _repo.GetAsync(DefaultUserId, item.Id);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetAsync_ByUserIdAndItemId_WhenItemDoesNotExist_ReturnsNull()
    {
        // Arrange - no items in database

        // Act
        var result = await _repo.GetAsync(DefaultUserId, 9999);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetAllAsync(userId)

    [Test]
    public async Task GetAllAsync_ByUserId_WhenItemsExist_ReturnsOnlyUsersItems()
    {
        // Arrange
        await AddSampleItemAsync(DefaultUserId, "T-Shirt 1");
        await AddSampleItemAsync(DefaultUserId, "T-Shirt 2");
        await AddSampleItemAsync("other-user-id", "Other T-Shirt");

        // Act
        var result = await _repo.GetAllAsync(DefaultUserId);

        // Assert
        using (new AssertionScope())
        {
            result.Should().HaveCount(2);
            result.Should().OnlyContain(i => i.UserId == DefaultUserId);
        }
    }

    [Test]
    public async Task GetAllAsync_ByUserId_WhenNoItemsExist_ReturnsEmptyList()
    {
        // Arrange - no items for this user

        // Act
        var result = await _repo.GetAllAsync(DefaultUserId);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion
}
