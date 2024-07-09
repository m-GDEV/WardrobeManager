using Microsoft.EntityFrameworkCore;
using WardrobeManager.Shared.Models;

namespace WardrobeManager.Api.Database;

public class DatabaseContext : DbContext, IDatabaseContext
{

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    public DbSet<ServerClothingItem> ClothingItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
