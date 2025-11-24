using System.ComponentModel.DataAnnotations.Schema;

namespace Ilmanar.Infra.Entities;

/// <summary>
/// Représente une section de contenu au sein d'un chapitre
/// </summary>
public class SectionEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Contenu de la section - peut être très volumineux (texte, JSON, HTML, etc.)
    /// </summary>
    [Column(TypeName = "TEXT")]
    public string Content { get; set; } = string.Empty;
    
    // Relation avec le type de section
    public int SectionTypeId { get; set; }
    public SectionTypeEntity SectionType { get; set; } = null!;
    
    public int DisplayOrder { get; set; }
    
    // Relation avec le chapitre (obligatoire)
    public Guid ChapterId { get; set; }
    public ChapterEntity Chapter { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

