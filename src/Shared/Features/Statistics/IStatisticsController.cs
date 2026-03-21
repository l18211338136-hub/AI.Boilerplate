namespace AI.Boilerplate.Shared.Features.Statistics;

[Route("api/v1/[controller]/[action]/")]
public interface IStatisticsController : IAppController
{
    [HttpGet("{packageId}")]
    Task<NugetStatsDto> GetNugetStats(string packageId, CancellationToken cancellationToken);

    [HttpGet, Route("https://github.com/l18211338136-hub/AI.Boilerplate"), ExternalApi]
    Task<GitHubStats> GetGitHubStats(CancellationToken cancellationToken) => default!;
}
