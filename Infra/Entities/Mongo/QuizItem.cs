using MongoDB.Bson.Serialization.Attributes;

namespace Ilmanar.Infra.Entities.Mongo;

[BsonDiscriminator("QuizItem")]
public class QuizItem : GameItem
{
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public string Type { get; set; } = string.Empty;

    public List<string> Options { get; set; } = new();

    public string CorrectAnswer { get; set; } = string.Empty;

    public string Explanation { get; set; } = string.Empty;
}
