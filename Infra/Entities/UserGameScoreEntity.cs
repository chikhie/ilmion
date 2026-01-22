using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ilmanar.Infra.Entities;

public class UserGameScoreEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    public UserEntity? User { get; set; }

    [Required]
    public Guid GameId { get; set; }
    // We don't have a navigation property for Game anymore as it is in Mongo
    // [ForeignKey("GameId")]
    // public GameEntity? Game { get; set; }

    public int Score { get; set; }

    public DateTime PlayedAt { get; set; } = DateTime.Now;
}
