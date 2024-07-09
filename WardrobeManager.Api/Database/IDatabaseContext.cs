using Microsoft.EntityFrameworkCore;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Database;
public interface IDatabaseContext
{
    DbSet<ServerClothingItem> ClothingItems { get; set; }
}