using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database.Models;

namespace WardrobeManager.Api.Database;
public interface IDatabaseContext
{
    DbSet<ClothingItem> ClothingItems { get; set; }
}