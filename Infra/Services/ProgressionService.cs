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
    private readonly IMongoCollection<Theme> _themeCollection;
    private readonly ILogger<ProgressionService> _logger;

    public ProgressionService(IOptions<MongoDbSettings> settings, ILogger<ProgressionService> logger)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _userProgressCollection = database.GetCollection<UserProgress>("UserProgress");
        _quizCollection = database.GetCollection<QuizItem>("QuizItems");
        _themeCollection = database.GetCollection<Theme>("Themes");
        _logger = logger;
    }

    public async Task SaveResultAsync(string userId, SaveQuizResultRequest request)
    {
        var progress = new UserProgress
        {
            UserId = userId,
            ThemeId = request.ThemeId,
            PartId = request.PartId,
            Points = request.Score, // Assuming Score = Points for now
            TotalQuestions = request.TotalQuestions,
            CorrectAnswers = request.Score, // Simplification: Score usually equals correct answers count in simple quizzes
            Duration = TimeSpan.FromSeconds(request.DurationSeconds),
            Date = DateTime.UtcNow,
            // Mastery Logic: > 80%?
            IsMastered = request.TotalQuestions > 0 && ((double)request.Score / request.TotalQuestions) >= 0.8
        };

        await _userProgressCollection.InsertOneAsync(progress);
    }

    public async Task<ProgressionViewModel> GetUserProgressionAsync(string userId)
    {
        var viewModel = new ProgressionViewModel();

        // 1. Get all Themes
        var themes = await _themeCollection.Find(_ => true).ToListAsync();

        foreach (var theme in themes)
        {
            var dto = new ModuleProgressionDto
            {
                ModuleId = theme.Id, // Use ThemeId as ModuleId identifier
                Label = theme.Title
            };

            // Get total questions count for this theme (all parts)
            dto.TotalQuestions = (int)await _quizCollection.CountDocumentsAsync(x => x.ThemeId == theme.Id);

            // Get User Progress for this theme
            // Fetch all attempts for this theme
            var userProgresses = await _userProgressCollection
                .Find(x => x.UserId == userId && x.ThemeId == theme.Id)
                .SortByDescending(x => x.Date)
                .ToListAsync();

            if (userProgresses.Any())
            {
                var latestAttempt = userProgresses.First();

                // Logic: 
                // TotalPoints: Sum of best score per Part? Or just sum of all experience?
                // Let's go with "Sum of all points earned" (XP style)
                dto.TotalPoints = userProgresses.Sum(x => x.Points);
                
                dto.LastPlayed = latestAttempt.Date;
                
                // Mastery: If any attempt on this theme (or majority of parts?) is mastered.
                // Simplified: If they have at least one Mastered attempt on the Theme (global mode) or all parts?
                // Let's look for any Mastered entry for this ThemeId.
                dto.IsMastered = userProgresses.Any(x => x.IsMastered);
                
                dto.QuestionsAnswered = userProgresses.Sum(x => x.CorrectAnswers); // Approximate

                if (dto.IsMastered) dto.CurrentMasteryLabel = "Maître";
                else if (dto.TotalPoints > 100) dto.CurrentMasteryLabel = "Avancé";
                else if (dto.TotalPoints > 20) dto.CurrentMasteryLabel = "Intermédiaire";
                else dto.CurrentMasteryLabel = "Novice";

                // Populate Parts Progression
                // Group by PartId to get stats per part
                var partGroups = userProgresses
                    .Where(x => !string.IsNullOrEmpty(x.PartId))
                    .GroupBy(x => x.PartId);


                foreach (var group in partGroups)
                {
                    var partId = group.Key;
                    var bestPartAttempt = group.OrderByDescending(x => x.Points).FirstOrDefault();
                    var latestPartAttempt = group.OrderByDescending(x => x.Date).FirstOrDefault();

                    if (bestPartAttempt != null)
                    {
                        // Get total questions for this specific part
                        var totalQuestionsForPart = (int)await _quizCollection.CountDocumentsAsync(x => x.PartId == partId);
                        
                        dto.Parts.Add(new PartProgressionDto
                        {
                            PartId = partId,
                            Score = bestPartAttempt.Points, // Best score
                            TotalQuestions = totalQuestionsForPart,
                            IsMastered = group.Any(x => x.IsMastered), // Mastered if any attempt is mastered
                            LastPlayed = latestPartAttempt?.Date ?? DateTime.MinValue,
                            Attempts = group.Count()
                        });
                    }
                }
            }

            viewModel.Modules.Add(dto);
        }

        // Global Stats
        viewModel.GlobalPoints = viewModel.Modules.Sum(m => m.TotalPoints);
        viewModel.ModulesMasteredCount = viewModel.Modules.Count(m => m.IsMastered);

        return viewModel;
    }
}
