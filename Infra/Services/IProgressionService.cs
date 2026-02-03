using Ilmanar.Api.Models;

namespace Ilmanar.Infra.Services;

public interface IProgressionService
{
    Task<ProgressionViewModel> GetUserProgressionAsync(string userId);
    Task SaveResultAsync(string userId, SaveQuizResultRequest request);
}
