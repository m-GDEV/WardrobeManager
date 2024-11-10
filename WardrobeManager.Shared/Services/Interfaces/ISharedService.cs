using WardrobeManager.Shared.Models;

namespace WardrobeManager.Shared.Services.Interfaces;
public interface ISharedService
{
    ServerClothingItem CreateDefaultServerClothingItem();
    public bool IsValidBase64(string input);
}
