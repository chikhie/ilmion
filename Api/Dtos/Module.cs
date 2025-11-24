namespace Ilmanar.Api.Dtos;

/// <summary>
/// DTO pour créer un nouveau module
/// </summary>
public class CreateModuleDto
{
    public string Label { get; set; } = string.Empty;
    public int SubjectId { get; set; }
}

/// <summary>
/// DTO pour mettre à jour un module
/// </summary>
public class UpdateModuleDto
{
    public string? Label { get; set; }
}

/// <summary>
/// DTO pour afficher un module
/// </summary>
public class ModuleResponseDto
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int ChapterCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
