using Ilmanar.Infra.Entities;
using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Ilmanar.Infra.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var quizService = scope.ServiceProvider.GetRequiredService<QuizService>();

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
            var existingQuiz = await context.Games.FirstOrDefaultAsync(g => g.Title == "Quiz Culture Générale");
            
            GameEntity quizGame;
            
            if (existingQuiz == null)
            {
                quizGame = new GameEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Quiz Culture Générale",
                    Description = "Testez vos connaissances sur la culture islamique et l'histoire de l'Andalousie.",
                    Type = GameType.Quiz,
                    Difficulty = "Medium",
                    IsPremium = false,
                    ContentJson = quizContent, // Keep for reference, but main data is in Mongo
                    CreatedAt = DateTime.Now
                };
                context.Games.Add(quizGame);
                await context.SaveChangesAsync(); // Save to get the ID persisted if needed, though we set it manually
            }
            else
            {
                quizGame = existingQuiz;
            }
            
            // Seed Mongo Questions
            if (quizService != null)
            {
                // Clear existing questions for this game to avoid duplicates on re-seed
                await quizService.RemoveQuestionsForGameAsync(quizGame.Id);
                
                try 
                {
                    using var doc = JsonDocument.Parse(quizContent);
                    if (doc.RootElement.TryGetProperty("questions", out var questionsElement))
                    {
                        var questions = questionsElement.EnumerateArray();
                        int order = 0;
                        foreach (var q in questions)
                        {
                            var newQ = new Question
                            {
                                GameId = quizGame.Id,
                                Text = q.GetProperty("text").GetString() ?? "",
                                Type = q.TryGetProperty("type", out var typeProp) ? typeProp.GetString() : "choice",
                                CorrectAnswer = q.GetProperty("correctAnswer").GetString(),
                                Explanation = q.TryGetProperty("explanation", out var expProp) ? expProp.GetString() : "",
                                Order = order++
                            };
                            
                            if (q.TryGetProperty("options", out var optsProp))
                            {
                                newQ.Options = optsProp.EnumerateArray().Select(o => o.GetString() ?? "").ToList();
                            }
                            
                            await quizService.CreateQuestionAsync(newQ);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding mongo content: {ex.Message}");
                }
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
