namespace Ilmanar.Api.Dtos;

public class DashboardStatsDto
{
    public int GlobalScore { get; set; }
    public int GlobalProgress { get; set; } // Percentage
    public int LearningTimeSeconds { get; set; } // Total or this week
    public string FormattedLearningTime { get; set; } = "";
}

public class UpdateGameScoreDto
{
    public Guid GameId { get; set; }
    public int Score { get; set; }
}
