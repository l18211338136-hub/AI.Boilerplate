using AI.Boilerplate.Server.Api.Infrastructure.Data.Seed;

namespace AI.Boilerplate.Server.Api.Features.AreaCodes;

public class AreaCodeSeeder : IDataSeeder
{
    public async Task SeedAsync(AppDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.AreaCodes.AnyAsync(cancellationToken) is false)
        {
            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "AreaCodes.sql");
            if (File.Exists(sqlFile))
            {
                var sqlScript = await File.ReadAllTextAsync(sqlFile);
                await dbContext.Database.ExecuteSqlRawAsync(sqlScript, cancellationToken);
            }
            else
            {
                Console.WriteLine($"SQL file not found: {sqlFile}");
            }
        }
    }
}
