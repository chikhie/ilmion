using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ilmanar.Infra.Entities.Mongo;

public class UserProgress
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string UserId { get; set; } = string.Empty;
    
    // Legacy support or if needed
    public string ModuleId { get; set; } = string.Empty; 

    public string? ThemeId { get; set; }
    public string? PartId { get; set; }

    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }

    public int Points { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public TimeSpan Duration { get; set; }

    public bool IsMastered { get; set; }
}
