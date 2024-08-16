using Microsoft.EntityFrameworkCore;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Services.Interfaces;
using WardrobeManager.Api.Database.Services.Interfaces;
using System.Diagnostics;

namespace WardrobeManager.Api.Database.Services.Implementation;

public class ClothingItemService : IClothingItemService
{
    private readonly DatabaseContext _context;
    private readonly IUserService _userService;
    private readonly ISharedService _sharedService;
    private readonly IFileService _fileService;

    public ClothingItemService(DatabaseContext databaseContext, IUserService userService, ISharedService sharedService, IFileService fileService)
    {
        _context = databaseContext;
        _userService = userService;
        _sharedService = sharedService;
        _fileService = fileService;
    }


    // ---- Methods for multiple clothing items ---
    public async Task<List<ServerClothingItem>?> GetAllClothing(int userId) {
        return _context.ClothingItems.Where(item => item.UserId == userId).ToList();
    }

    // ---- Methods for one clothing item ---
    public async Task<ServerClothingItem?> GetClothingItem(int userId, int itemId) {
        return await _context.ClothingItems.Where(item => item.Id == itemId && item.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task AddClothingItem(int userId, NewOrEditedClothingItemDTO newItem)
    {
        if (newItem == null)
        {
            throw new Exception("NewOrEditedClothingItemDTO is null");
        }

        Guid? newItemGuid = null;
        if (_sharedService.IsValidBase64(newItem.ImageBase64)) {
            newItemGuid = Guid.NewGuid();
            // decode and save file to place on disk with guid as name
            await _fileService.SaveImage(newItemGuid, newItem.ImageBase64);
        }

        ServerClothingItem newClothingItem  = new ServerClothingItem
            (
             newItem.Name,
             newItem.Category,
             newItem.Season,
             newItem.WearLocation,
             newItem.Favourited,
             newItem.DesiredTimesWornBeforeWash,
             newItemGuid
            );

        newClothingItem.UserId = userId;
        await _context.ClothingItems.AddAsync(newClothingItem);
        await _context.SaveChangesAsync();
    }


    public async Task UpdateClothingItem(int userId, int itemId, NewOrEditedClothingItemDTO editedItem)
    {
        var dbRecord = await GetClothingItem(userId, itemId);

        // There was no 'old' item to edit
        if (dbRecord == null) {
            throw new Exception("Cannot find old item");
        }
        if (editedItem == null) {
            throw new Exception("editedItem is null");
        }

        dbRecord.Name = editedItem.Name;
        dbRecord.Category = editedItem.Category;
        dbRecord.Season = editedItem.Season;
        dbRecord.Favourited = editedItem.Favourited;
        dbRecord.WearLocation = editedItem.WearLocation;
        dbRecord.DesiredTimesWornBeforeWash = editedItem.DesiredTimesWornBeforeWash;
        dbRecord.DateUpdated = DateTime.UtcNow;

        // If its null we assume they do not want to change the existing image
        if (editedItem.ImageBase64 != null) {
            Guid? editedItemGuid = null;
            if (_sharedService.IsValidBase64(editedItem.ImageBase64)) {
                editedItemGuid = Guid.NewGuid();
                // decode and save file to place on disk with guid as name
                await _fileService.SaveImage(editedItemGuid, editedItem.ImageBase64);
                dbRecord.ImageGuid = editedItemGuid;
            }
        }

        _context.ClothingItems.Update(dbRecord);
        await _context.SaveChangesAsync();
    }

    public async Task CallMethodOnClothingItem(int userId, int itemId, ActionType type)
    {
        var dbRecord = await GetClothingItem(userId, itemId);

        // There was no 'old' item to edit
        if (dbRecord == null) {
            throw new Exception("Cannot find old item");
        }

        switch(type) {
            case ActionType.Wear:
                dbRecord.Wear();
                break;
            case ActionType.Wash:
                dbRecord.Wash();
                break;
        }

        _context.ClothingItems.Update(dbRecord);
        await _context.SaveChangesAsync();
    }


    public async Task DeleteClothingItem(int userId, int itemId)
    {
        var itemToDelete = await GetClothingItem(userId, itemId);

        if (itemToDelete == null) {
            throw new Exception("Cannot find item to delete");
        }

        _context.ClothingItems.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }

}
