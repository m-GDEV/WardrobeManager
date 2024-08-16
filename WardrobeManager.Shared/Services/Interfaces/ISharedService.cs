using WardrobeManager.Shared.Models;

namespace WardrobeManager.Shared.Services.Interfaces;
public interface ISharedService
{
    bool IsValid(ServerClothingItem item);
    ServerClothingItem CreateDefaultServerClothingItem();
    public bool IsValidBase64(string input);

}
