#region

using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Repositories;
using WardrobeManager.Api.Repositories.Interfaces;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.StaticResources;

#endregion

namespace WardrobeManager.Api.Services.Implementation;

public class ClothingService(IClothingRepository clothingRepository)
    : IClothingService
{


    // ---- Methods for multiple clothing items ---
    public async Task<List<ClothingItem>?> GetAllClothingAsync(string userId)
    {
        return await clothingRepository.GetAllAsync(userId);
    }

    // ---- Methods for one clothing item ---
    public async Task<ClothingItem?> GetClothingItemAsync(string userId, int itemId)
    {
       return await clothingRepository.GetAsync(userId, itemId);
    }


}
