namespace Ilmanar.Infra.Entities;

/// <summary>
/// Représente un module au sein d'une matière
/// Un module contient plusieurs chapitres
/// </summary>
public class ModuleEntity
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } // Ordre d'affichage dans la matière
    
    // Relation avec la matière
    public int SubjectId { get; set; }
    public SubjectEntity Subject { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Relations
    public ICollection<ChapterEntity> Chapters { get; set; } = new List<ChapterEntity>();
}
