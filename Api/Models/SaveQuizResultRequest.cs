using System.ComponentModel.DataAnnotations;

namespace Ilmanar.Api.Models;

public class SaveQuizResultRequest
{
    [Required]
    public int Score { get; set; }

    [Required]
    public int TotalQuestions { get; set; }

    public string? ThemeId { get; set; }
    public string? PartId { get; set; }

    public int DurationSeconds { get; set; }
}
