using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ilmanar.Infra.Entities;

public class UserLearningLogEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;
    
    [ForeignKey("UserId")]
    public virtual UserEntity User { get; set; } = null!;

    public DateTime Date { get; set; } // The date of activity (stripped time usually)
    
    public int DurationSeconds { get; set; } // Duration in seconds
    
    public Guid? ModuleId { get; set; } // Optional: track which module was studied
    
    [ForeignKey("ModuleId")]
    public virtual ModuleEntity? Module { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

