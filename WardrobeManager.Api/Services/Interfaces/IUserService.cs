using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUser(int id);
    Task<bool> DoesUserExist(int id);
    Task CreateUser();
    Task UpdateUser(int userId, EditedUserDTO editedUser);
    Task DeleteUser(int userId);
    Task<bool> DoesAdminUserExist();
    Task<(bool, string)> CreateAdminIfMissing(string email, string password);
}