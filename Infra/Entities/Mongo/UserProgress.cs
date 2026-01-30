using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ilmanar.Infra.Entities.Mongo;

public class UserProgress
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string UserId { get; set; } = string.Empty;

    public string ModuleId { get; set; } = string.Empty; // e.g. "tawheed"

    public int Points { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public TimeSpan Duration { get; set; }

    public bool IsMastered { get; set; }
}
