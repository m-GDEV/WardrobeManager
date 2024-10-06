using WardrobeManager.Shared.Models;
using WardrobeManager.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace WardrobeManager.Api.Database;

public class DatabaseInitializer
{
    public static void Initialize(DatabaseContext context)
    {
        // We don't need to create any data (users, clothing items, etc) in the db for now 

        // Create db and applies migrations (useful when running on new environment, don't need to do this manually)
        context.Database.Migrate();
    }
}
