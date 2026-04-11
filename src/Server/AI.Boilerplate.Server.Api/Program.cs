using AI.Boilerplate.Server.Api.Features.Products;

namespace AI.Boilerplate.Server.Api;

public static partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        AppEnvironment.Set(builder.Environment.EnvironmentName);

        builder.Configuration.AddSharedConfigurations();

        builder.WebHost.UseSentry(configureOptions: options => builder.Configuration.GetRequiredSection("Logging:Sentry").Bind(options));

        builder.Services.AddSharedProjectServices(builder.Configuration);
        builder.AddServerApiProjectServices();

        var app = builder.Build();

        if (builder.Environment.IsDevelopment())
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync(); // It's recommended to start using ef-core migrations.
        }

        app.ConfigureMiddlewares();

        // 初始化产品向量化后台任务
        using var jobScope = app.Services.CreateScope();
        var productEmbeddingJobRunner = jobScope.ServiceProvider.GetRequiredService<ProductEmbeddingJobRunner>();
        productEmbeddingJobRunner.Initialize();

        await app.RunAsync();
    }
}
