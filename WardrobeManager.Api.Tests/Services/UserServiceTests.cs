using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Identity;
using Moq;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Services.Implementation;
using WardrobeManager.Api.Tests.Helpers;
using WardrobeManager.Shared.DTOs;

namespace WardrobeManager.Api.Tests.Services;

public class UserServiceTests
{
    private Mock<UserManager<User>> _mockUserManager;
    private Mock<RoleManager<IdentityRole>> _mockRoleManager;
    private UserService _service;

    [SetUp]
    public void Setup()
    {
        var userStore = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(
            userStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        var roleStore = new Mock<IRoleStore<IdentityRole>>();
        _mockRoleManager = new Mock<RoleManager<IdentityRole>>(
            roleStore.Object, null!, null!, null!, null!);

        _service = new UserService(_mockUserManager.Object, _mockRoleManager.Object);
    }

    #region GetUser

    [Test]
    public async Task GetUser_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = "test-id" };
        _mockUserManager.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);

        // Act
        var result = await _service.GetUser("test-id");

        // Assert
        _mockUserManager.Verify(m => m.FindByIdAsync("test-id"), Times.Once);
        result.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task GetUser_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        _mockUserManager.Setup(m => m.FindByIdAsync("nonexistent")).ReturnsAsync((User?)null);

        // Act
        var result = await _service.GetUser("nonexistent");

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region DoesUserExist

    [Test]
    public async Task DoesUserExist_WhenUserExists_ReturnsTrue()
    {
        // Arrange
        var user = new User { Id = "test-id" };
        _mockUserManager.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);

        // Act
        var result = await _service.DoesUserExist("test-id");

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task DoesUserExist_WhenUserDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _mockUserManager.Setup(m => m.FindByIdAsync("nonexistent")).ReturnsAsync((User?)null);

        // Act
        var result = await _service.DoesUserExist("nonexistent");

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region UpdateUser

    [Test]
    public async Task UpdateUser_WhenCalled_UpdatesUserFields()
    {
        // Arrange
        var user = new User { Id = "test-id", Name = "Old Name", ProfilePictureBase64 = "old-pic" };
        var editedUser = new EditedUserDTO("New Name", "new-pic");
        _mockUserManager.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);
        _mockUserManager.Setup(m => m.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        // Act
        await _service.UpdateUser("test-id", editedUser);

        // Assert
        _mockUserManager.Verify(m => m.UpdateAsync(It.Is<User>(u =>
            u.Name == "New Name" && u.ProfilePictureBase64 == "new-pic")), Times.Once);
    }

    #endregion

    #region DeleteUser

    [Test]
    public async Task DeleteUser_WhenCalled_DeletesUser()
    {
        // Arrange
        var user = new User { Id = "test-id" };
        _mockUserManager.Setup(m => m.FindByIdAsync("test-id")).ReturnsAsync(user);
        _mockUserManager.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Act
        await _service.DeleteUser("test-id");

        // Assert
        _mockUserManager.Verify(m => m.DeleteAsync(user), Times.Once);
    }

    #endregion

    #region DoesAdminUserExist

    [Test]
    public async Task DoesAdminUserExist_WhenAdminExists_ReturnsTrue()
    {
        // Arrange
        var adminUser = new User { Id = "admin-id" };
        var users = new AsyncQueryHelper<User>(new List<User> { adminUser });
        _mockUserManager.Setup(m => m.Users).Returns(users);
        _mockRoleManager.Setup(m => m.RoleExistsAsync("Admin")).ReturnsAsync(true);
        _mockUserManager.Setup(m => m.IsInRoleAsync(adminUser, "Admin")).ReturnsAsync(true);

        // Act
        var result = await _service.DoesAdminUserExist();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task DoesAdminUserExist_WhenNoAdminExists_ReturnsFalse()
    {
        // Arrange
        var regularUser = new User { Id = "user-id" };
        var users = new AsyncQueryHelper<User>(new List<User> { regularUser });
        _mockUserManager.Setup(m => m.Users).Returns(users);
        _mockRoleManager.Setup(m => m.RoleExistsAsync("Admin")).ReturnsAsync(true);
        _mockUserManager.Setup(m => m.IsInRoleAsync(regularUser, "Admin")).ReturnsAsync(false);

        // Act
        var result = await _service.DoesAdminUserExist();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task DoesAdminUserExist_WhenNoUsersExist_ReturnsFalse()
    {
        // Arrange
        var users = new AsyncQueryHelper<User>(new List<User>());
        _mockUserManager.Setup(m => m.Users).Returns(users);
        _mockRoleManager.Setup(m => m.RoleExistsAsync("Admin")).ReturnsAsync(true);

        // Act
        var result = await _service.DoesAdminUserExist();

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region CreateAdminIfMissing

    [Test]
    public async Task CreateAdminIfMissing_WhenAdminAlreadyExists_ReturnsFalseWithMessage()
    {
        // Arrange - admin user already exists
        var adminUser = new User { Id = "admin-id" };
        var users = new AsyncQueryHelper<User>(new List<User> { adminUser });
        _mockUserManager.Setup(m => m.Users).Returns(users);
        _mockRoleManager.Setup(m => m.RoleExistsAsync("Admin")).ReturnsAsync(true);
        _mockUserManager.Setup(m => m.IsInRoleAsync(adminUser, "Admin")).ReturnsAsync(true);

        // Act
        var result = await _service.CreateAdminIfMissing("admin@test.com", "password");

        // Assert
        using (new AssertionScope())
        {
            result.Item1.Should().BeFalse();
            result.Item2.Should().Be("Admin user already exists!");
        }
    }

    [Test]
    public async Task CreateAdminIfMissing_WhenAdminDoesNotExist_CreatesAdminAndReturnsTrue()
    {
        // Arrange - no admin user exists
        var users = new AsyncQueryHelper<User>(new List<User>());
        _mockUserManager.Setup(m => m.Users).Returns(users);
        _mockRoleManager.Setup(m => m.RoleExistsAsync("Admin")).ReturnsAsync(true);
        _mockUserManager
            .Setup(m => m.CreateAsync(It.IsAny<User>(), "SecurePass1!"))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager
            .Setup(m => m.AddToRoleAsync(It.IsAny<User>(), "Admin"))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _service.CreateAdminIfMissing("admin@test.com", "SecurePass1!");

        // Assert
        using (new AssertionScope())
        {
            result.Item1.Should().BeTrue();
            result.Item2.Should().Be("Admin user created!");
        }
    }

    #endregion

    #region CreateUser

    [Test]
    public async Task CreateUser_WhenCalled_CreatesNewUserWithDefaultClothingItems()
    {
        // Arrange
        _mockUserManager
            .Setup(m => m.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _service.CreateUser();

        // Assert
        _mockUserManager.Verify(m => m.CreateAsync(It.Is<User>(u =>
            u.ServerClothingItems.Count == 2)), Times.Once);
    }

    #endregion
}
