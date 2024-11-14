﻿using Microsoft.EntityFrameworkCore;
using WardrobeManager.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WardrobeManager.Api.Database.Models;

namespace WardrobeManager.Api.Database;

public class DatabaseContext : IdentityDbContext<AppUser>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    public DbSet<AppUser> AppUsers { get; set; }
    public new DbSet<User> Users { get; set; }
    public DbSet<ServerClothingItem> ClothingItems { get; set; }
    public DbSet<Log> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(e=> e.ServerClothingItems)
            .WithOne(e=> e.User)
            .HasForeignKey(e=> e.UserId)
            .OnDelete(DeleteBehavior.Cascade) // deletes a user's clothing items when they are deleted
            .IsRequired();
    }
}
