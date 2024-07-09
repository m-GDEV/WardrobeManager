using WardrobeManager.Shared.Models;

namespace WardrobeManager.Shared.Services.Interfaces;
public interface ISharedService
{
    bool IsValid(ClientClothingItem item);
    bool IsValid(ServerClothingItem item);

}