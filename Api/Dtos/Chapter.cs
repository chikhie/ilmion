namespace Ilmanar.Api.Dtos;

/// <summary>
/// DTO pour créer un nouveau chapitre
/// </summary>
public class CreateChapterDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public int DurationMinutes { get; set; }
    public Guid ModuleId { get; set; }
    public string? VideoId { get; set; }
    public string? VideoUrl { get; set; }
    public string? Content { get; set; }
}

/// <summary>
/// DTO pour mettre à jour un chapitre
/// </summary>
public class UpdateChapterDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? DisplayOrder { get; set; }
    public int? DurationMinutes { get; set; }
    public string? VideoId { get; set; }
    public string? VideoUrl { get; set; }
    public string? Content { get; set; }
}

/// <summary>
/// DTO pour afficher un chapitre
/// </summary>
public class ChapterResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public int DurationMinutes { get; set; }
    public bool HasAccess { get; set; } // Si l'utilisateur a accès (abonnement OU premier chapitre)
    public bool IsFree { get; set; } // Si c'est le premier chapitre gratuit
    public Guid ModuleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public int SubjectId { get; set; } // ID de la matière
    public string SubjectName { get; set; } = string.Empty; // Nom de la matière
    public string? VideoId { get; set; }
    public string? VideoUrl { get; set; }
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

