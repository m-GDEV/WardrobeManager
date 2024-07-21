using Microsoft.EntityFrameworkCore;
using WardrobeManager.Shared.Models;
using WardrobeManager.Api.Database.Services.Interfaces;

namespace WardrobeManager.Api.Database.Services.Interfaces;

public interface IUserService
{
    public Task CreateUser(string Auth0Id);
    public Task<bool> DoesUserExist(string Auth0Id);
    public Task<User> GetUser(string Auth0Id);
}
