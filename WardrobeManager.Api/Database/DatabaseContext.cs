#region

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WardrobeManager.Api.Database.Entities;

#endregion

namespace WardrobeManager.Api.Database;

public class DatabaseContext : IdentityDbContext<User>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    public new DbSet<User> Users { get; set; }
    public DbSet<ServerClothingItem> ClothingItems { get; set; }
    public DbSet<Log> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(e=> e.ServerClothingItems)
            .WithOne(e=> e.User)
            .HasForeignKey(e => e.UserId)
            .HasPrincipalKey(e => e.PrimaryKeyId)
            .OnDelete(DeleteBehavior.Cascade) // deletes a user's clothing items when they are deleted
            .IsRequired();
        
        // Configure all entities that implement IDatabaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IDatabaseEntity).IsAssignableFrom(entityType.ClrType) && entityType.ClrType != new User().GetType())
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasKey(nameof(IDatabaseEntity.PrimaryKeyId));
            }
        }
    }
}
