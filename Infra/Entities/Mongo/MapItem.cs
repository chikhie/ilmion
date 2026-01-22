using MongoDB.Bson.Serialization.Attributes;

namespace Ilmanar.Infra.Entities.Mongo;

[BsonDiscriminator("MapItem")]
public class MapItem : GameItem
{
    // The target coordinates the player needs to find
    public double X { get; set; }
    public double Y { get; set; }

    // How close they need to be to count as correct (optional)
    public double? AcceptableRadius { get; set; }

    // Optional explanation or location info to show after reveal
    public string LocationName { get; set; } = string.Empty;
}
