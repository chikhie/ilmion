namespace Ilmanar.Infra.Entities.Mongo;

public enum GameType
{
    MapPlacement,
    Translation,
    Genealogy,
    Quiz
}

public class Game
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public GameType Type { get; set; }

    public string ThumbnailPath { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
