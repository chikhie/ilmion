namespace Ilmanar.Infra.Entities;

/// <summary>
/// Représente un type de section (Text, Image, Video, Quiz, etc.)
/// </summary>
public class SectionTypeEntity
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    
    // Relations
    public ICollection<SectionEntity> Sections { get; set; } = new List<SectionEntity>();
}

