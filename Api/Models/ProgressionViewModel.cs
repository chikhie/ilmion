namespace Ilmanar.Api.Models;

public class ProgressionViewModel
{
    public int GlobalPoints { get; set; }
    public int ModulesMasteredCount { get; set; }
    public List<ModuleProgressionDto> Modules { get; set; } = new();
}
