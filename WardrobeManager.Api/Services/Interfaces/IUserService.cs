using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUser(string userId);
    Task<bool> DoesUserExist(string userId);
    Task CreateUser();
    Task UpdateUser(string userId, EditedUserDTO editedUser);
    Task DeleteUser(string userId);
    Task<bool> DoesAdminUserExist();
    Task<(bool, string)> CreateAdminIfMissing(string email, string password);
}