using Ilmanar.Infra.Entities.Mongo;
using System.Collections.Generic;

namespace Ilmanar.Infra.Data.SeedData;

public class ThemeSeedDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<ThemeSubjectSeedDto> Subjects { get; set; } = new();
}

public class ThemeSubjectSeedDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<ThemePartSeedDto> Parts { get; set; } = new();
}

public class ThemePartSeedDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<QuizItem> Quiz { get; set; } = new();
}
