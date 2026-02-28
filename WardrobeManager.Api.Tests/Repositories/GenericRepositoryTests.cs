using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Repositories.Implementation;
using WardrobeManager.Shared.Enums;

namespace WardrobeManager.Api.Tests.Repositories;

public class GenericRepositoryTests
{
    private DatabaseContext _context;
    private GenericRepository<Log> _repo;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options);
        _repo = new GenericRepository<Log>(_context);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.DisposeAsync();
    }

    #region GetAsync

    [Test]
    public async Task GetAsync_WhenEntityExists_ReturnsEntity()
    {
        // Arrange
        var log = new Log("Test Title", "Test Desc", LogType.Info, LogOrigin.Backend);
        await _context.Logs.AddAsync(log);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetAsync(log.Id);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result!.Title.Should().Be("Test Title");
        }
    }

    [Test]
    public async Task GetAsync_WhenEntityDoesNotExist_ReturnsNull()
    {
        // Arrange - empty database

        // Act
        var result = await _repo.GetAsync(999);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetAllAsync

    [Test]
    public async Task GetAllAsync_WhenEntitiesExist_ReturnsAllEntities()
    {
        // Arrange
        await _context.Logs.AddRangeAsync(
            new Log("Log 1", "Desc 1", LogType.Info, LogOrigin.Backend),
            new Log("Log 2", "Desc 2", LogType.Error, LogOrigin.Frontend)
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _repo.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetAllAsync_WhenEmpty_ReturnsEmptyList()
    {
        // Arrange - empty database

        // Act
        var result = await _repo.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region CreateAsync

    [Test]
    public async Task CreateAsync_WhenCalled_AddsEntityToDatabase()
    {
        // Arrange
        var log = new Log("New Log", "Description", LogType.Warning, LogOrigin.Database);

        // Act
        await _repo.CreateAsync(log);
        await _repo.SaveAsync();

        // Assert
        var savedLog = await _context.Logs.FirstOrDefaultAsync(l => l.Title == "New Log");
        savedLog.Should().NotBeNull();
    }

    #endregion

    #region CreateManyAsync

    [Test]
    public async Task CreateManyAsync_WhenCalled_AddsAllEntitiesToDatabase()
    {
        // Arrange
        var logs = new List<Log>
        {
            new("Log A", "Desc A", LogType.Info, LogOrigin.Backend),
            new("Log B", "Desc B", LogType.Info, LogOrigin.Backend),
            new("Log C", "Desc C", LogType.Info, LogOrigin.Backend),
        };

        // Act
        await _repo.CreateManyAsync(logs);
        await _repo.SaveAsync();

        // Assert
        var count = await _context.Logs.CountAsync();
        count.Should().Be(3);
    }

    #endregion

    #region Remove

    [Test]
    public async Task Remove_WhenEntityExists_RemovesFromDatabase()
    {
        // Arrange
        var log = new Log("To Remove", "Desc", LogType.Info, LogOrigin.Unknown);
        await _context.Logs.AddAsync(log);
        await _context.SaveChangesAsync();

        // Act
        _repo.Remove(log);
        await _repo.SaveAsync();

        // Assert
        var deletedLog = await _context.Logs.FindAsync(log.Id);
        deletedLog.Should().BeNull();
    }

    #endregion

    #region Update

    [Test]
    public async Task Update_WhenEntityIsModified_SavesChanges()
    {
        // Arrange
        var log = new Log("Original", "Desc", LogType.Info, LogOrigin.Backend);
        await _context.Logs.AddAsync(log);
        await _context.SaveChangesAsync();

        // Act
        log.Title = "Updated";
        _repo.Update(log);
        await _repo.SaveAsync();

        // Assert
        var updatedLog = await _context.Logs.FindAsync(log.Id);
        updatedLog!.Title.Should().Be("Updated");
    }

    #endregion

    #region SaveAsync

    [Test]
    public async Task SaveAsync_WhenCalled_PersistsChanges()
    {
        // Arrange
        var log = new Log("Unsaved Log", "Desc", LogType.Info, LogOrigin.Backend);
        await _context.Logs.AddAsync(log);

        // Changes not yet saved
        var countBeforeSave = await _context.Logs.CountAsync();

        // Act
        await _repo.SaveAsync();

        // Assert
        var countAfterSave = await _context.Logs.CountAsync();
        countAfterSave.Should().Be(countBeforeSave + 1);
    }

    #endregion
}
