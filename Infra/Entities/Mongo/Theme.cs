using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Ilmanar.Infra.Entities.Mongo;

[BsonIgnoreExtraElements]
public class Theme
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ThemeSubject> Subjects { get; set; } = new();
}

[BsonIgnoreExtraElements]
public class ThemeSubject
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ThemePart> Parts { get; set; } = new();
}

[BsonIgnoreExtraElements]
public class ThemePart
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    // Quiz items are now stored in a separate collection
}
