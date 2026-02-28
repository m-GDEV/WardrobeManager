#region

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Database.Entities;
using WardrobeManager.Api.Repositories;
using WardrobeManager.Api.Repositories.Interfaces;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.DTOs;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.StaticResources;

#endregion

namespace WardrobeManager.Api.Services.Implementation;

public class ClothingService(
    IClothingRepository clothingRepository,
    IMapper mapper,
    IFileService fileService,
    ILogger<ClothingService> logger,
    IMiscMethods miscMethods
)
    : IClothingService
{
    // ---- Methods for multiple clothing items ---
    public async Task<List<ClothingItemDTO>?> GetAllClothingAsync(string userId)
    {
        var res = await clothingRepository.GetAllAsync(userId);
        return mapper.Map<List<ClothingItemDTO>>(res);
    }

    // ---- Methods for one clothing item ---
    public async Task<ClothingItemDTO?> GetClothingItemAsync(string userId, int itemId)
    {
        var res = await clothingRepository.GetAsync(userId, itemId);
        return mapper.Map<ClothingItemDTO>(res);
    }

    public async Task AddNewClothingItem(string userId, NewClothingItemDTO newClothingItem)
    {
        var res = mapper.Map<ClothingItem>(newClothingItem);
        Guid? newItemGuid = null;
        if (miscMethods.IsValidBase64(newClothingItem.ImageBase64))
        {
            newItemGuid = Guid.NewGuid();
            // decode and save file to place on disk with guid as name
            await fileService.SaveImage(newItemGuid, newClothingItem.ImageBase64!);
        }

        res.UserId = userId;
        res.ImageGuid = newItemGuid;
        await clothingRepository.CreateAsync(res);
        await clothingRepository.SaveAsync();
    }

    public async Task RemoveClothingItem(string userId, int itemId)
    {
        var res = await clothingRepository.GetAsync(userId, itemId);
        if (res != null)
        {
            clothingRepository.Remove(res);
            await clothingRepository.SaveAsync();
            if (res.ImageGuid != null)
            {
                await fileService.DeleteImage((Guid)res.ImageGuid);
            }
        }
        else
        {
            logger.LogInformation($"Clothing item {itemId} not found");
        }
    }
}