using System.Text.Json;
using Ilmanar.Infra.Entities;
using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ilmanar.Infra.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var quizService = scope.ServiceProvider.GetRequiredService<QuizService>();


        // Seeding logic removed as QuizService now loads data internally from JSON on startup.
        await Task.CompletedTask;



        await context.SaveChangesAsync();
    }
}
