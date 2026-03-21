using Microsoft.Extensions.Logging;
using Azure.Monitor.OpenTelemetry.Exporter;
using AI.Boilerplate.Client.Maui.Infrastructure.Services;
using AI.Boilerplate.Client.Core.Infrastructure.Services.HttpMessageHandlers;

namespace AI.Boilerplate.Client.Maui;

public static partial class MauiProgram
{
    public static void ConfigureServices(this MauiAppBuilder builder)
    {
        // Services being registered here can get injected in Maui (Android, iOS, macOS, Windows)
        var services = builder.Services;
        var configuration = builder.Configuration;
        services.AddClientCoreProjectServices(builder.Configuration);

        services.AddTransient<MainPage>();

        services.AddScoped<IWebAuthnService, MauiWebAuthnService>();
        services.AddScoped<IExceptionHandler, MauiExceptionHandler>();
        services.AddScoped<IAppUpdateService, MauiAppUpdateService>();
        services.AddScoped<IBitDeviceCoordinator, MauiDeviceCoordinator>();
        services.AddScoped<IExternalNavigationService, MauiExternalNavigationService>();

        services.AddScoped<HttpClient>(sp =>
        {
            var handlerFactory = sp.GetRequiredService<HttpMessageHandlersChainFactory>();
            var httpClient = new HttpClient(handlerFactory.Invoke())
            {
                BaseAddress = new Uri(configuration.GetServerAddress(), UriKind.Absolute),
                Timeout = TimeSpan.FromMinutes(10)
            };
            if (Uri.TryCreate(configuration.GetServerAddress(), UriKind.Absolute, out var origin))
            {
                httpClient.DefaultRequestHeaders.Add("X-Origin", origin.ToString());
            }
            return httpClient;
        });

        services.AddSingleton<IStorageService, MauiStorageService>();
        var settings = new ClientMauiSettings();
        configuration.Bind(settings);
        services.AddSingleton(sp =>
        {
            return settings;
        });
        services.AddSingleton(ITelemetryContext.Current!);
        services.AddSingleton<ILocalHttpServer, MauiLocalHttpServer>();

        services.AddMauiBlazorWebView();
        services.AddBlazorWebViewDeveloperTools();

        builder.Logging.ConfigureLoggers(configuration);

        builder.Logging.AddEventSourceLogger();

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;

            if (string.IsNullOrEmpty(settings.ApplicationInsights?.ConnectionString) is false)
            {
                options.AddAzureMonitorLogExporter(o =>
                {
                    o.ConnectionString = settings.ApplicationInsights.ConnectionString;
                });
            }
        });

        if (AppPlatform.IsWindows)
        {
            builder.Logging.AddEventLog(options => configuration.GetRequiredSection("Logging:EventLog").Bind(options));
        }

        services.AddOptions<ClientMauiSettings>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        #if Android
        services.AddClientMauiProjectAndroidServices(builder.Configuration);
#elif iOS
        services.AddClientMauiProjectIosServices(builder.Configuration);
#elif Mac
        services.AddClientMauiProjectMacCatalystServices(builder.Configuration);
#elif Windows
        services.AddClientMauiProjectWindowsServices(builder.Configuration);
#endif
            }
}
