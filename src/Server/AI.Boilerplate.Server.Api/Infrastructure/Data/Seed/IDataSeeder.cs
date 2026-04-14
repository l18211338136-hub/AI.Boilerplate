using AI.Boilerplate.Server.Api.Infrastructure.Data;

namespace AI.Boilerplate.Server.Api.Infrastructure.Data.Seed;

public interface IDataSeeder
{
    Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken);
}
