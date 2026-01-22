namespace Ilmanar.Infra.Entities.Mongo;

public abstract class GameItem
{
    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public List<string> Options { get; set; } = [];
    public string Explanation { get; set; } = string.Empty;
}
