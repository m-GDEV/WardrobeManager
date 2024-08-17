using Microsoft.EntityFrameworkCore;
using WardrobeManager.Shared.Models;
using WardrobeManager.Api.Database.Services.Interfaces;

namespace WardrobeManager.Api.Database.Services.Interfaces;

public interface IUserService
{
    // Auth0Id methods (used in middleware)
    public Task<User?> GetUser(string Auth0Id);
    public Task CreateUser(string Auth0Id);
    public Task<bool> DoesUserExist(string Auth0Id);

    // Normal methods for endpoints
    public Task<User?> GetUser(int userId);
    Task UpdateUser(int userId, EditedUserDTO editedUser);
    Task DeleteUser(int userId);
}
