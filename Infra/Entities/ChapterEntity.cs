using System.ComponentModel.DataAnnotations.Schema;

namespace Ilmanar.Infra.Entities;

/// <summary>
/// Représente un chapitre d'apprentissage au sein d'un module
/// </summary>
public class ChapterEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } // Ordre d'affichage dans le module
    public int DurationMinutes { get; set; } // Durée estimée du chapitre en minutes
    
    // Vidéo associée au chapitre (ID pour référence ou URL directe pour HLS/YouTube)
    public string? VideoId { get; set; } // ID de la vidéo (ex: YouTube ID ou Mux Asset ID)
    public string? VideoUrl { get; set; } // URL HLS ou YouTube directe
    
    // Relation avec le module (obligatoire)
    public Guid ModuleId { get; set; }
    public ModuleEntity Module { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Relations
    public string? Content { get; set; }
    public ICollection<SectionEntity> Sections { get; set; } = new List<SectionEntity>();
}

