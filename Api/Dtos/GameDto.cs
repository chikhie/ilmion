using Ilmanar.Infra.Entities;

namespace Ilmanar.Api.Dtos;

public class GameDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public GameType Type { get; set; }
    public string ThumbnailPath { get; set; } = string.Empty;
    public List<Ilmanar.Infra.Entities.Mongo.Question>? Questions { get; set; }
}
