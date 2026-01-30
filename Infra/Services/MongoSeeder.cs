using System.Text.Json;
using Ilmanar.Infra.Entities.Mongo;
using Ilmanar.Infra.Settings;
using Ilmanar.Infra.Data.SeedData; // Added
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;

namespace Ilmanar.Infra.Services;

public class MongoSeeder
{
    private readonly IMongoCollection<Theme> _themeCollection;
    private readonly IMongoCollection<Mastery> _masteryCollection;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<MongoSeeder> _logger;

    public MongoSeeder(IOptions<MongoDbSettings> settings, IWebHostEnvironment env, ILogger<MongoSeeder> logger)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _themeCollection = database.GetCollection<Theme>("Themes");
        _masteryCollection = database.GetCollection<Mastery>("Masteries");
        _env = env;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SeedMasteriesAsync();
        await SeedThemesAsync();
    }

    private async Task SeedMasteriesAsync()
    {
        var masteriesToSeed = new List<Mastery>
        {
            new Mastery { Label = "Débutant", ModuleId = "tawheed" },
            new Mastery { Label = "Intermédiaire", ModuleId = "tawheed" },
            new Mastery { Label = "Avancé", ModuleId = "tawheed" }
        };

        foreach (var m in masteriesToSeed)
        {
            var exists = await _masteryCollection.Find(x => x.Label == m.Label && x.ModuleId == m.ModuleId).AnyAsync();
            if (!exists)
            {
                await _masteryCollection.InsertOneAsync(m);
                _logger.LogInformation($"Seeding Mastery: {m.Label} for module {m.ModuleId}");
            }
        }
    }

    private async Task SeedThemesAsync()
    {
        try
        {
            var dbName = _themeCollection.Database.DatabaseNamespace.DatabaseName;
            _logger.LogInformation($"Seeding to Database: {dbName}");
            
            // Drop QuizItems collection to ensure clean state (schema changes, etc.)
            await _themeCollection.Database.DropCollectionAsync("QuizItems");
            _logger.LogInformation("Dropped QuizItems collection for clean seed.");

            var filePath = Path.Combine(_env.ContentRootPath, "Infra", "Data", "SeedData", "themes.json");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning($"Themes seed file not found: {filePath}");
                return;
            }

            var content = await File.ReadAllTextAsync(filePath);
            var themesDto = JsonSerializer.Deserialize<List<ThemeSeedDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (themesDto != null)
            {
                var quizCollection = _themeCollection.Database.GetCollection<QuizItem>("QuizItems");

                foreach (var themeDto in themesDto)
                {
                    // 0. Resolve Theme ID
                    var existingTheme = await _themeCollection.Find(t => t.Title == themeDto.Title).FirstOrDefaultAsync();
                    if (existingTheme != null)
                    {
                        themeDto.Id = existingTheme.Id;
                    }
                    else if (string.IsNullOrEmpty(themeDto.Id))
                    {
                        themeDto.Id = ObjectId.GenerateNewId().ToString();
                    }

                    // 1. Prepare Theme Entity
                    var themeSubjects = new List<ThemeSubject>();
                    
                    foreach(var subjectDto in themeDto.Subjects)
                    {
                        if (string.IsNullOrEmpty(subjectDto.Id)) subjectDto.Id = Guid.NewGuid().ToString();
                        
                        var themeParts = new List<ThemePart>();

                        foreach(var partDto in subjectDto.Parts)
                        {
                            if (string.IsNullOrEmpty(partDto.Id)) partDto.Id = Guid.NewGuid().ToString();

                            // 2. Extract and Save Quiz Items
                            if (partDto.Quiz != null && partDto.Quiz.Any())
                            {
                                _logger.LogInformation($"Found {partDto.Quiz.Count} questions for part {partDto.Title}");
                                foreach (var q in partDto.Quiz)
                                {
                                    q.ThemeId = themeDto.Id;
                                    q.SubjectId = subjectDto.Id;
                                    q.PartId = partDto.Id;
                                    if(string.IsNullOrEmpty(q.Id)) q.Id = ObjectId.GenerateNewId().ToString();
                                }
                                
                                // Delete existing questions for this part to avoid duplicates on re-seed
                                await quizCollection.DeleteManyAsync(q => q.ThemeId == themeDto.Id && q.SubjectId == subjectDto.Id && q.PartId == partDto.Id);
                                await quizCollection.InsertManyAsync(partDto.Quiz);
                                _logger.LogInformation($"Inserted {partDto.Quiz.Count} questions into QuizItems collection.");
                            }
                            else
                            {
                                _logger.LogWarning($"No questions found in JSON for part {partDto.Title}");
                            }

                            themeParts.Add(new ThemePart 
                            { 
                                Id = partDto.Id, 
                                Title = partDto.Title, 
                                Description = partDto.Description 
                            });
                        }

                        themeSubjects.Add(new ThemeSubject
                        {
                            Id = subjectDto.Id,
                            Title = subjectDto.Title,
                            Parts = themeParts
                        });
                    }

                    // 3. Save Theme
                    var theme = new Theme 
                    { 
                        Id = themeDto.Id, 
                        Title = themeDto.Title, 
                        Description = themeDto.Description,
                        Subjects = themeSubjects
                    };

                    if (existingTheme == null)
                    {
                        await _themeCollection.InsertOneAsync(theme);
                         _logger.LogInformation($"Inserted Theme: {theme.Title}");
                    }
                    else
                    {
                        await _themeCollection.ReplaceOneAsync(t => t.Id == existingTheme.Id, theme);
                        _logger.LogInformation($"Updated Theme: {theme.Title}");
                    }
                } // End foreach

                var quizCount = await quizCollection.CountDocumentsAsync(_ => true);
                var themeCount = await _themeCollection.CountDocumentsAsync(_ => true);
                _logger.LogInformation($"Final Counts - Themes: {themeCount}, QuizItems: {quizCount}");

                // DEBUG: Try to read one item
                var firstItem = await quizCollection.Find(_ => true).FirstOrDefaultAsync();
                if (firstItem != null)
                {
                    _logger.LogInformation($"Successfully read item: {firstItem.Id} - {firstItem.Text}");
                }
                else
                {
                    _logger.LogError("Failed to read any items back from QuizItems collection!");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding themes");
        }
    }
}
