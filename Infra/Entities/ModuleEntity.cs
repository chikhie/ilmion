using System.ComponentModel.DataAnnotations.Schema;

namespace Ilmanar.Infra.Entities;

/// <summary>
/// Représente un module d'apprentissage au sein d'une matière
/// </summary>
public class ModuleEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } // Ordre d'affichage dans la matière
    public int DurationMinutes { get; set; } // Durée estimée du module en minutes
    
    // Prix du module
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;
    
    public bool IsFree { get; set; } = false; // Si gratuit, pas besoin de paiement
    
    // Vidéo associée au module
    public Guid? VideoId { get; set; }
    public VideoEntity? Video { get; set; }
    
    // Relation avec la matière
    public int SubjectId { get; set; }
    public SubjectEntity Subject { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Relations
    public string? Content { get; set; }
    public ICollection<ModulePurchaseEntity> Purchases { get; set; } = new List<ModulePurchaseEntity>();
}

