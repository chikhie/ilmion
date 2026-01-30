namespace Ilmanar.Api.Models;

public class ModuleProgressionDto
{
    public string ModuleId { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public int TotalPoints { get; set; }
    public bool IsMastered { get; set; }
    public string CurrentMasteryLabel { get; set; } = "Novice";
    public DateTime? LastPlayed { get; set; }
    public int QuestionsAnswered { get; set; }
    public int TotalQuestions { get; set; }
}
