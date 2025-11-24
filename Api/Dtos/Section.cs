namespace Ilmanar.Api.Dtos;

/// <summary>
/// DTO pour créer une nouvelle section
/// </summary>
public class CreateSectionDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int SectionTypeId { get; set; } // 1=Text, 2=Image, 3=Video, 4=Quiz, 5=Code, 6=Exercise
    public int DisplayOrder { get; set; }
    public Guid ChapterId { get; set; }
}

/// <summary>
/// DTO pour mettre à jour une section
/// </summary>
public class UpdateSectionDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int? SectionTypeId { get; set; }
    public int? DisplayOrder { get; set; }
}

/// <summary>
/// DTO pour afficher une section
/// </summary>
public class SectionResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int SectionTypeId { get; set; }
    public string SectionTypeLabel { get; set; } = string.Empty; // "Texte", "Image", etc.
    public int DisplayOrder { get; set; }
    public Guid ChapterId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

