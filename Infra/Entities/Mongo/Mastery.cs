using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ilmanar.Infra.Entities.Mongo;

public class Mastery
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Label { get; set; } = string.Empty;

    public string ModuleId { get; set; } = string.Empty; // e.g. "tawheed"
}
