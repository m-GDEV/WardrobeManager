using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Services.Interfaces;

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

    public Task<bool> DoesAdminUserExist();
    
    // bool: succeeded?, string: text description
    public Task<(bool, string)> CreateAdminIfMissing(string email, string password);
}
