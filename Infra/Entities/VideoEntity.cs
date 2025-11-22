using System.ComponentModel.DataAnnotations;

namespace Ilmanar.Infra.Entities;

/// <summary>
/// Représente une vidéo éducative
/// </summary>
public class VideoEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public string VideoResolution { get; set; } = string.Empty;
    public string VideoFormat { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string ThumbnailPath { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
