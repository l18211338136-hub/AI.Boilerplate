using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using AI.Boilerplate.Server.Api.Infrastructure.Data;

namespace AI.Boilerplate.Tests.Infrastructure;

[TestClass]
public partial class TestsAssemblyInitializer
{

    [AssemblyInitialize]
    public static async Task Initialize(TestContext testContext)
    {
        await using var testServer = new AppTestServer();

        await testServer.Build().Start(testContext.CancellationToken);

        await InitializeDatabase(testServer);
    }


    private static async Task InitializeDatabase(AppTestServer testServer)
    {
        if (testServer.WebApp.Environment.IsDevelopment())
        {
            await using var scope = testServer.WebApp.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync(); // It's recommended to start using ef-core migrations.
        }
    }

}
