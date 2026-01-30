using Ilmanar.Api.Models;
using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Ilmanar.Infra.Services;

public class ProgressionService : IProgressionService
{
    private readonly IMongoCollection<UserProgress> _userProgressCollection;
    private readonly IMongoCollection<QuizItem> _quizCollection;
    private readonly IMongoCollection<Mastery> _masteryCollection;
    private readonly ILogger<ProgressionService> _logger;

    public ProgressionService(IOptions<MongoDbSettings> settings, ILogger<ProgressionService> logger)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _userProgressCollection = database.GetCollection<UserProgress>("UserProgress");
        _quizCollection = database.GetCollection<QuizItem>("QuizItems");
        _masteryCollection = database.GetCollection<Mastery>("Masteries");
        _logger = logger;
    }

    public async Task<ProgressionViewModel> GetUserProgressionAsync(string userId)
    {
        var viewModel = new ProgressionViewModel();

        // 1. Get all unique ModuleIds from QuizItems
        // Using a distinct query to find all available modules
        var distinctModuleIds = await _quizCollection.Distinct(x => x.ModuleId, FilterDefinition<QuizItem>.Empty).ToListAsync();
        
        // Remove nulls if any
        var modules = distinctModuleIds.Where(m => !string.IsNullOrEmpty(m)).ToList();

        foreach (var moduleId in modules)
        {
            var dto = new ModuleProgressionDto
            {
                ModuleId = moduleId,
                Label = Capitalize(moduleId) // Simple formatting for now
            };

            // Get total questions count for this module
            dto.TotalQuestions = (int)await _quizCollection.CountDocumentsAsync(x => x.ModuleId == moduleId);

            // Get User Progress for this module
            // We might have multiple entries if we track history, but usually we want the latest or aggregated status.
            // For this design, let's assume we want to sum up points or show the "best" status.
            // Or if UserProgress is one-per-module-per-user (state), we fetch that.
            // Let's assume UserProgress is a log of attempts. We want to aggregate.
            
            var userProgresses = await _userProgressCollection
                .Find(x => x.UserId == userId && x.ModuleId == moduleId)
                .SortByDescending(x => x.Date)
                .ToListAsync();

            if (userProgresses.Any())
            {
                var bestAttempt = userProgresses.OrderByDescending(x => x.Points).First();
                var latestAttempt = userProgresses.First(); 

                dto.TotalPoints = userProgresses.Sum(x => x.Points); // Cumulative points or best score? Let's do Cumulative for "XP" feel.
                dto.LastPlayed = latestAttempt.Date;
                dto.IsMastered = userProgresses.Any(x => x.IsMastered);
                
                // Determine mastery label based on points or explicit mastery
                // For now, let's just fetch the Masteries for this module and see if we can calculate it
                // Or simply use the logic: if IsMastered -> "Expert", else determine by score.
                // Simplified:
                if (dto.IsMastered) dto.CurrentMasteryLabel = "Maître";
                else if (dto.TotalPoints > 100) dto.CurrentMasteryLabel = "Avancé"; // Arbitrary threshold
                else if (dto.TotalPoints > 50) dto.CurrentMasteryLabel = "Intermédiaire";
                else dto.CurrentMasteryLabel = "Novice";
                
                // Track questions answered (unique)
                // This would require storing answered question IDs in UserProgress. 
                // For now, let's roughly estimate or leave as 0 if not tracked.
                // If we want to be precise, UserProgress should have a list of answered IDs.
            }

            viewModel.Modules.Add(dto);
        }

        // Global Stats
        viewModel.GlobalPoints = viewModel.Modules.Sum(m => m.TotalPoints);
        viewModel.ModulesMasteredCount = viewModel.Modules.Count(m => m.IsMastered);

        return viewModel;
    }

    private string Capitalize(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return char.ToUpper(input[0]) + input.Substring(1);
    }
}
