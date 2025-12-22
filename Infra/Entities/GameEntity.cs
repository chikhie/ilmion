using System.ComponentModel.DataAnnotations;

namespace Ilmanar.Infra.Entities;

public enum GameType
{
    MapPlacement,
    Translation,
    Genealogy,
    Quiz
}

public class GameEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public GameType Type { get; set; }
    
    public string Difficulty { get; set; } = "Medium"; // Easy, Medium, Hard
    
    // Flexible JSON content to store game data interactively
    // Map: { "targetLat": 24.46, "targetLng": 39.61, "hint": "..." }
    // Translation: { "arabic": "Salam", "french": "Paix", "options": [...] }
    public string ContentJson { get; set; } = "{}";
    
    public bool IsPremium { get; set; } = true;
    
    [MaxLength(255)]
    public string ThumbnailPath { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
