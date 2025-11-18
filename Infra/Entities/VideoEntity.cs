namespace KitabStock.Infra.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class VideoEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; } = TimeSpan.Zero;
    public string VideoResolution { get; set; } = string.Empty;
    public string VideoFormat { get; set; } = string.Empty;
    public double FrameRate { get; set; } = 0;
    public string FilenameDownload { get; set; } = string.Empty;
    public string ColorSpace { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    [MaxLength(255)]
    public string Link { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string ThumbnailPath { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;
}