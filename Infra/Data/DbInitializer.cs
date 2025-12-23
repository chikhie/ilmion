using Ilmanar.Infra.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ilmanar.Infra.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure database is created (do NOT use EnsureDeletedAsync in production/dev if you want to keep data)
        await context.Database.EnsureCreatedAsync();

        // Seed Roles
        string[] roleNames = { "Admin", "User", "Premium" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Seed Admin User
        var adminEmail = "admin@ilmanar.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new UserEntity
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "Ilmanar",
                Bio = "Administrateur de la plateforme."
            };
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

    // Seed Student User
        var studentEmail = "student@ilmanar.com";
        var studentUser = await userManager.FindByEmailAsync(studentEmail);
        if (studentUser == null)
        {
            studentUser = new UserEntity
            {
                UserName = studentEmail,
                Email = studentEmail,
                EmailConfirmed = true,
                FirstName = "Étudiant",
                LastName = "Test",
                Bio = "Étudiant passionné par l'arabe."
            };
            var result = await userManager.CreateAsync(studentUser, "Student123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(studentUser, "User");
            }
        }

        // Seed Games
        // 1. Quiz Game
        var quizContentPath = Path.Combine(Directory.GetCurrentDirectory(), "Infra", "Data", "SeedData", "culture_quiz.json");
        if (File.Exists(quizContentPath))
        {
            var quizContent = await File.ReadAllTextAsync(quizContentPath);
            if (!context.Games.Any(g => g.Title == "Quiz Culture Générale"))
            {
                var quizGame = new GameEntity
                {
                    Title = "Quiz Culture Générale",
                    Description = "Testez vos connaissances sur la culture islamique et l'histoire de l'Andalousie.",
                    Type = GameType.Quiz,
                    Difficulty = "Medium",
                    IsPremium = false,
                    ContentJson = quizContent, // Raw JSON content
                    CreatedAt = DateTime.Now
                };
                context.Games.Add(quizGame);
            }
        }

        // 2. Map Game
        var mapContentPath = Path.Combine(Directory.GetCurrentDirectory(), "Infra", "Data", "SeedData", "map_game.json");
        if (File.Exists(mapContentPath))
        {
            var mapContent = await File.ReadAllTextAsync(mapContentPath);
             if (!context.Games.Any(g => g.Title == "Exploration d'Al-Andalus"))
            {
                var mapGame = new GameEntity
                {
                    Title = "Exploration d'Al-Andalus",
                    Description = "Placez les villes historiques d'Al-Andalus sur la carte.",
                    Type = GameType.MapPlacement,
                    Difficulty = "Easy",
                    IsPremium = false,
                    ContentJson = mapContent,
                    CreatedAt = DateTime.Now
                };
                context.Games.Add(mapGame);
            }
        }
        
        await context.SaveChangesAsync();
    }
}
