using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ilmanar.Api.Models.Mongo;

public class Question
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public Guid GameId { get; set; } // Link to SQL GameEntity ID

    public string Text { get; set; } = string.Empty;

    public string Type { get; set; } = "choice"; // choice, number

    public List<string> Options { get; set; } = new();

    public string CorrectAnswer { get; set; } = string.Empty;

    public string Explanation { get; set; } = string.Empty;
    
    public int Order { get; set; }
}
