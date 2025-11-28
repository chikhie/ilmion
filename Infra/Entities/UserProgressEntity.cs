using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ilmanar.Infra.Entities;

public class UserProgressEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;
    
    [ForeignKey("UserId")]
    public virtual UserEntity User { get; set; } = null!;

    [Required]
    public Guid SectionId { get; set; }
    
    [ForeignKey("SectionId")]
    public virtual SectionEntity Section { get; set; } = null!;

    public bool IsCompleted { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

