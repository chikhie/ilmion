using MongoDB.Bson.Serialization.Attributes;

namespace Ilmanar.Infra.Entities.Mongo;

// [BsonDiscriminator("QuizItem")] removed
public class QuizItem : GameItem
{
    [BsonIgnoreIfNull]
    public string? ModuleId { get; set; }

// Lang inherited from GameItem
}
