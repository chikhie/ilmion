namespace Ilmanar.Api.Models;

public class PartProgressionDto
{
    public string PartId { get; set; } = string.Empty;
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public bool IsMastered { get; set; }
    public DateTime LastPlayed { get; set; }
    public int Attempts { get; set; }
}
