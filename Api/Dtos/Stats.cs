namespace Ilmanar.Api.Dtos;

public class DashboardStatsDto
{
    public List<FollowedModuleDto> FollowedModules { get; set; } = new();
    public int GlobalProgress { get; set; } // Percentage
    public int LearningTimeSeconds { get; set; } // Total or this week
    public string FormattedLearningTime { get; set; } = "";
}

public class FollowedModuleDto
{
    public Guid Id { get; set; }
    public string Label { get; set; } = "";
    public string SubjectName { get; set; } = "";
    public int ChapterCount { get; set; }
    public int Progress { get; set; } // Percentage
    public string? ImageUrl { get; set; }
}

public class UpdateProgressDto
{
    public Guid SectionId { get; set; }
    public bool IsCompleted { get; set; }
}

public class LogTimeDto
{
    public int DurationSeconds { get; set; }
    public Guid? ModuleId { get; set; }
}

