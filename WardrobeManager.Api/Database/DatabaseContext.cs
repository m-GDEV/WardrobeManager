using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database.Models;

namespace WardrobeManager.Api.Database;

public class DatabaseContext : DbContext, IDatabaseContext
{

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    public DbSet<ClothingItem> ClothingItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
