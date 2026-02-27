using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Endpoints;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Tests.Endpoints;

public class UserEndpointsTests
{
    private Mock<IUserService> _mockUserService;

    [SetUp]
    public void Setup()
    {
        _mockUserService = new Mock<IUserService>();
    }

    #region GetUser

    [Test]
    public async Task GetUser_WhenCalled_ReturnsOkWithUser()
    {
        // Arrange
        var user = new User { Id = "test-user", Name = "Test User" };
        var httpContext = new DefaultHttpContext();
        httpContext.Items["user"] = user;

        // Act
        var result = await UserEndpoints.GetUser(_mockUserService.Object, httpContext);

        // Assert
        var okResult = result as Ok<User>;
        using (new AssertionScope())
        {
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeSameAs(user);
        }
    }

    #endregion

    #region EditUser

    [Test]
    public async Task EditUser_WhenCalled_CallsUpdateUserAndReturnsOk()
    {
        // Arrange
        var user = new User { Id = "test-user" };
        var httpContext = new DefaultHttpContext();
        httpContext.Items["user"] = user;
        var editedUser = new EditedUserDTO("New Name", "new-pic");
        _mockUserService
            .Setup(s => s.UpdateUser(user.Id, editedUser))
            .Returns(Task.CompletedTask);

        // Act
        var result = await UserEndpoints.EditUser(editedUser, _mockUserService.Object, httpContext);

        // Assert
        _mockUserService.Verify(s => s.UpdateUser(user.Id, editedUser), Times.Once);
        var okResult = result as Ok<string>;
        okResult.Should().NotBeNull();
    }

    #endregion

    #region DeleteUser

    [Test]
    public async Task DeleteUser_WhenCalled_CallsDeleteUserAndReturnsOk()
    {
        // Arrange
        var user = new User { Id = "test-user" };
        var httpContext = new DefaultHttpContext();
        httpContext.Items["user"] = user;
        _mockUserService
            .Setup(s => s.DeleteUser(user.Id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await UserEndpoints.DeleteUser(_mockUserService.Object, httpContext);

        // Assert
        _mockUserService.Verify(s => s.DeleteUser(user.Id), Times.Once);
        var okResult = result as Ok<string>;
        okResult.Should().NotBeNull();
    }

    #endregion
}
