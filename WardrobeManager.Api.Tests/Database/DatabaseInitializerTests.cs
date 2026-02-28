using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;

namespace WardrobeManager.Api.Tests.Database;

public class DatabaseInitializerTests
{
    private Mock<RoleManager<IdentityRole>> _mockRoleManager;
    private DatabaseContext _context;
    private SqliteConnection _connection;

    [SetUp]
    public void Setup()
    {
        // Use a persistent in-memory SQLite connection so migrations can run
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var dbOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite(_connection)
            .Options;
        _context = new DatabaseContext(dbOptions);

        // Setup mock RoleManager
        var roleStore = new Mock<IRoleStore<IdentityRole>>();
        _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
            roleStore.Object, null, null, null, null);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

    private IServiceScope BuildScope()
    {
        var mockServiceProvider = new Mock<IServiceProvider>();
        mockServiceProvider
            .Setup(sp => sp.GetService(typeof(DatabaseContext)))
            .Returns(_context);
        mockServiceProvider
            .Setup(sp => sp.GetService(typeof(RoleManager<IdentityRole>)))
            .Returns(_mockRoleManager.Object);

        var mockScope = new Mock<IServiceScope>();
        mockScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);
        return mockScope.Object;
    }

    [Test]
    public async Task InitializeAsync_WhenNoUsersExist_CreatesAdminAndUserRoles()
    {
        // Arrange
        _mockRoleManager.Setup(r => r.RoleExistsAsync("Admin")).ReturnsAsync(false);
        _mockRoleManager.Setup(r => r.RoleExistsAsync("User")).ReturnsAsync(false);
        _mockRoleManager
            .Setup(r => r.CreateAsync(It.IsAny<IdentityRole>()))
            .ReturnsAsync(IdentityResult.Success);

        var scope = BuildScope();

        // Act
        await DatabaseInitializer.InitializeAsync(scope);

        // Assert
        _mockRoleManager.Verify(r => r.CreateAsync(It.Is<IdentityRole>(role => role.Name == "Admin")), Times.Once);
        _mockRoleManager.Verify(r => r.CreateAsync(It.Is<IdentityRole>(role => role.Name == "User")), Times.Once);
    }

    [Test]
    public async Task InitializeAsync_WhenRolesAlreadyExist_DoesNotRecreateRoles()
    {
        // Arrange
        _mockRoleManager.Setup(r => r.RoleExistsAsync("Admin")).ReturnsAsync(true);
        _mockRoleManager.Setup(r => r.RoleExistsAsync("User")).ReturnsAsync(true);

        var scope = BuildScope();

        // Act
        await DatabaseInitializer.InitializeAsync(scope);

        // Assert
        _mockRoleManager.Verify(r => r.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
    }

    [Test]
    public async Task InitializeAsync_WhenUsersExist_SkipsRoleCreation()
    {
        // Arrange - Apply migrations first so we can add a user
        await _context.Database.MigrateAsync();
        var user = new User { Id = "existing-user", UserName = "existinguser" };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var scope = BuildScope();

        // Act - InitializeAsync will call MigrateAsync (no-op, already migrated) then exit early
        await DatabaseInitializer.InitializeAsync(scope);

        // Assert - RoleManager should NOT be called since users already exist (early return)
        _mockRoleManager.Verify(r => r.RoleExistsAsync(It.IsAny<string>()), Times.Never);
    }
}
