using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database;
using WardrobeManager.Api.Services.Interfaces;
using WardrobeManager.Shared.Enums;
using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Services.Interfaces;

namespace WardrobeManager.Api.Services.Implementation;

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
    public async Task<List<ServerClothingItem>?> GetAllClothing(int userId)
    {
        return _context.ClothingItems.Where(item => item.UserId == userId).ToList();
    }
    public async Task<List<ServerClothingItem>?> GetFilteredClothing(int userId, FilterModel model)
    {
        var items = await GetAllClothing(userId);
        
        if (items == null)
        {
            return items;
        }

        // Filtering logic:
        // booleans: if the bool is not activated I assume you don't care about that boolean
        //           eg: if model.Favourited it false I don't assume you want to see non-favourited items,
        //           i assume you don't care about that field and this method will return items regardless of 
        //           of their 'Favourited' status
        // ints:     If these values are 0 we assume the user doesn't want to filter based on them

        var filteredItems = items
            // Logic for this might be fucked - Selects items with/without an image based on model 
            .Where(item => model.SearchQuery == string.Empty || item.Name.ToLower().Contains(model.SearchQuery.ToLower()))
            .Where(item => !model.HasImage || (item.ImageGuid != null) == model.HasImage)
            .Where(item => !model.Favourited || item.Favourited == model.Favourited)
            // Selects items with 'DateAdded' two weeks or less from the current time, only if recently added is true
            .Where(item => !model.RecentlyAdded || (DateTime.UtcNow - item.DateAdded) <= TimeSpan.FromDays(14))
            .Where(item => model.Category == ClothingCategory.None || item.Category == model.Category) // Ignore if None
            .Where(item => model.Season == Season.None || item.Season == model.Season) // Ignore if None
            .Where(item => model.DateAddedFrom == null || item.DateAdded >= model.DateAddedFrom)
            .Where(item => model.DateAddedTo == null || item.DateAdded <= model.DateAddedTo)
            .Where(item => model.LastWornFrom == null || item.LastWorn >= model.LastWornFrom)
            .Where(item => model.LastWornTo == null || item.LastWorn <= model.LastWornTo)
            .Where(item => model.LastEditedFrom == null || item.DateUpdated >= model.LastEditedFrom)
            .Where(item => model.LastEditedTo == null || item.DateUpdated <= model.LastEditedTo)
            .Where(item => model.TimesWorn == 0 || item.TimesWornTotal >= model.TimesWorn)
            .Where(item => model.TimesWornSinceWash == 0 || item.TimesWornSinceWash >= model.TimesWornSinceWash);
    

        // Apply sorting rules (thank you chatgpt)
        Func<ServerClothingItem, object>? sortKey = model.SortBy switch
        {
            SortByCategories.Category => item => item.Category,
            SortByCategories.Season => item => item.Season,
            SortByCategories.TimesWorn => item => item.TimesWornTotal,
            SortByCategories.DateAdded => item => item.DateAdded,
            _ => null // For SortByCategories.None or unsupported cases
        };

        if (sortKey == null) return filteredItems.ToList(); // No sorting if no valid key

        return model.IsAscending
            ? filteredItems.OrderBy(sortKey).ToList()
            : filteredItems.OrderByDescending(sortKey).ToList();
    }



    // ---- Methods for one clothing item ---
    public async Task<ServerClothingItem?> GetClothingItem(int userId, int itemId)
    {
        return await _context.ClothingItems.Where(item => item.Id == itemId && item.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task AddClothingItem(int userId, NewOrEditedClothingItemDTO newItem)
    {
        if (newItem == null)
        {
            throw new Exception("NewOrEditedClothingItemDTO is null");
        }

        Guid? newItemGuid = null;
        if (_sharedService.IsValidBase64(newItem.ImageBase64))
        {
            newItemGuid = Guid.NewGuid();
            // decode and save file to place on disk with guid as name
            await _fileService.SaveImage(newItemGuid, newItem.ImageBase64);
        }

        ServerClothingItem newClothingItem = new ServerClothingItem
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
        if (dbRecord == null)
        {
            throw new Exception("Cannot find old item");
        }
        if (editedItem == null)
        {
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
        if (editedItem.ImageBase64 != null)
        {
            Guid? editedItemGuid = null;
            if (_sharedService.IsValidBase64(editedItem.ImageBase64))
            {
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
        if (dbRecord == null)
        {
            throw new Exception("Cannot find old item");
        }

        switch (type)
        {
            case ActionType.Wear:
                dbRecord.Wear();
                break;
            case ActionType.Wash:
                dbRecord.Wash();
                break;
            default:
                // Here because if the enum expands this will be out of date
                throw new Exception("Action not recognized");
        }

        _context.ClothingItems.Update(dbRecord);
        await _context.SaveChangesAsync();
    }


    public async Task DeleteClothingItem(int userId, int itemId)
    {
        var itemToDelete = await GetClothingItem(userId, itemId);

        if (itemToDelete == null)
        {
            throw new Exception("Cannot find item to delete");
        }

        _context.ClothingItems.Remove(itemToDelete);
        await _context.SaveChangesAsync();
    }

}
