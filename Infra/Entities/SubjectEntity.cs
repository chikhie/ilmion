namespace Ilmanar.Infra.Entities;

/// <summary>
/// Représente une matière (Mathématiques, Physique, Chimie, Biologie, Informatique)
/// </summary>
public class SubjectEntity
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    
    // Relations
    public ICollection<ModuleEntity> Modules { get; set; } = new List<ModuleEntity>();
}

