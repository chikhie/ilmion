using System.Text.Json;
using Ilmanar.Infra.Entities;
using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ilmanar.Infra.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // Apply Migrations (SQLite)
        await context.Database.MigrateAsync();

        // Mongo Seeding
        try 
        {
            var mongoSeeder = scope.ServiceProvider.GetRequiredService<MongoSeeder>();
            await mongoSeeder.SeedAsync();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>(); // Use Program logger for static class convenience
            logger.LogError(ex, "Mongo Seeding Failed");
        }

        await context.SaveChangesAsync();
    }
}
