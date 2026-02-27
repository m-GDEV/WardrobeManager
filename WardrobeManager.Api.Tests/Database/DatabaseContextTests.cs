using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Tests.Database;

public class DatabaseContextTests
{
    private DatabaseContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.DisposeAsync();
    }

    [Test]
    public void DatabaseContext_HasUsersDbSet()
    {
        // Assert
        _context.Users.Should().NotBeNull();
    }

    [Test]
    public void DatabaseContext_HasClothingItemsDbSet()
    {
        // Assert
        _context.ClothingItems.Should().NotBeNull();
    }

    [Test]
    public void DatabaseContext_HasLogsDbSet()
    {
        // Assert
        _context.Logs.Should().NotBeNull();
    }

    [Test]
    public async Task DatabaseContext_WhenUserAddedWithClothingItems_SavesRelationship()
    {
        // Arrange
        var user = new User
        {
            Id = "test-user",
            UserName = "testuser",
            ServerClothingItems = new List<ClothingItem>
            {
                new ClothingItem("T-Shirt", ClothingCategory.TShirt, Season.Fall,
                    WearLocation.HomeAndOutside, false, 3, null)
            }
        };

        // Act
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Assert
        var savedUser = await _context.Users
            .Include(u => u.ServerClothingItems)
            .FirstOrDefaultAsync(u => u.Id == "test-user");

        using (new AssertionScope())
        {
            savedUser.Should().NotBeNull();
            savedUser!.ServerClothingItems.Should().HaveCount(1);
            savedUser.ServerClothingItems.First().Name.Should().Be("T-Shirt");
        }
    }

    [Test]
    public async Task DatabaseContext_WhenUserDeleted_CascadeDeletesClothingItems()
    {
        // Arrange
        var user = new User
        {
            Id = "cascade-test-user",
            UserName = "cascadeuser",
            ServerClothingItems = new List<ClothingItem>
            {
                new ClothingItem("Item 1", ClothingCategory.Jeans, Season.Fall,
                    WearLocation.Outside, false, 0, null)
            }
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        // Assert - cascade delete should have removed the clothing items too
        var clothingItemCount = await _context.ClothingItems
            .Where(c => c.UserId == "cascade-test-user")
            .CountAsync();
        clothingItemCount.Should().Be(0);
    }

    [Test]
    public async Task DatabaseContext_CanAddAndRetrieveLogs()
    {
        // Arrange
        var log = new Log("Test log", "Description", LogType.Info, LogOrigin.Backend);

        // Act
        await _context.Logs.AddAsync(log);
        await _context.SaveChangesAsync();

        // Assert
        var savedLog = await _context.Logs.FirstOrDefaultAsync(l => l.Title == "Test log");
        using (new AssertionScope())
        {
            savedLog.Should().NotBeNull();
            savedLog!.Description.Should().Be("Description");
            savedLog.Type.Should().Be(LogType.Info);
        }
    }
}
