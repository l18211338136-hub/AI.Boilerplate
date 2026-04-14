using AI.Boilerplate.Server.Api.Infrastructure.Data;

namespace AI.Boilerplate.Server.Api.Infrastructure.Data.Seed;

public class SeedDataService(IEnumerable<IDataSeeder> seeders, AppDbContext dbContext)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(dbContext, cancellationToken);
        }
    }
}
