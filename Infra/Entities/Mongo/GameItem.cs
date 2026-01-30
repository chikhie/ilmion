namespace Ilmanar.Infra.Entities.Mongo;

[MongoDB.Bson.Serialization.Attributes.BsonIgnoreExtraElements]
public abstract class GameItem
{
    [MongoDB.Bson.Serialization.Attributes.BsonId]
    [MongoDB.Bson.Serialization.Attributes.BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; } = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public List<string> Options { get; set; } = [];
    public string Explanation { get; set; } = string.Empty;

    // Foreign Keys / Metadata
    public string ThemeId { get; set; } = string.Empty;
    public string SubjectId { get; set; } = string.Empty;
    public string PartId { get; set; } = string.Empty;
    public string Lang { get; set; } = "fr";
}
