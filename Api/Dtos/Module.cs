namespace Ilmanar.Api.Dtos;

/// <summary>
/// DTO pour créer un nouveau module
/// </summary>
public class CreateModuleDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; } = 0;
    public bool IsFree { get; set; } = false;
    public int SubjectId { get; set; }
    public Guid? VideoId { get; set; }
    public string? Content { get; set; }
}

/// <summary>
/// DTO pour mettre à jour un module
/// </summary>
public class UpdateModuleDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? DisplayOrder { get; set; }
    public int? DurationMinutes { get; set; }
    public decimal? Price { get; set; }
    public bool? IsFree { get; set; }
    public Guid? VideoId { get; set; }
    public string? Content { get; set; }
}

/// <summary>
/// DTO pour afficher un module
/// </summary>
public class ModuleResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public bool IsFree { get; set; }
    public bool HasAccess { get; set; } // Si l'utilisateur a accès au module
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public Guid? VideoId { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

