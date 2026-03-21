using OpenTelemetry.Logs;
using Microsoft.Extensions.Logging;
using Azure.Monitor.OpenTelemetry.Exporter;
using AI.Boilerplate.Client.Windows.Infrastructure.Services;
using AI.Boilerplate.Client.Core.Infrastructure.Services.HttpMessageHandlers;

namespace AI.Boilerplate.Client.Windows;

public static partial class Program
{
    public static void AddClientWindowsProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Services being registered here can get injected in windows project only.
        services.AddClientCoreProjectServices(configuration);

        services.AddScoped<IWebAuthnService, WindowsWebAuthnService>();
        services.AddScoped<IExceptionHandler, WindowsExceptionHandler>();
        services.AddScoped<IAppUpdateService, WindowsAppUpdateService>();
        services.AddScoped<IBitDeviceCoordinator, WindowsDeviceCoordinator>();

        services.AddScoped<HttpClient>(sp =>
        {
            var handlerFactory = sp.GetRequiredService<HttpMessageHandlersChainFactory>();
            var httpClient = new HttpClient(handlerFactory.Invoke())
            {
                BaseAddress = new Uri(configuration.GetServerAddress(), UriKind.Absolute),
                Timeout = TimeSpan.FromMinutes(10)
            };
            if (sp.GetRequiredService<ClientWindowsSettings>().WebAppUrl is Uri origin)
            {
                httpClient.DefaultRequestHeaders.Add("X-Origin", origin.ToString());
            }
            return httpClient;
        });

        services.AddSingleton(sp => configuration);
        services.AddSingleton<IStorageService, WindowsStorageService>();
        services.AddSingleton<ILocalHttpServer, WindowsLocalHttpServer>();

        ClientWindowsSettings settings = new();
        configuration.Bind(settings);
        services.AddSingleton(sp =>
        {
            return settings;
        });
        services.AddSingleton(ITelemetryContext.Current!);
        services.AddSingleton<IPushNotificationService, WindowsPushNotificationService>();

        services.AddWindowsFormsBlazorWebView();
        services.AddBlazorWebViewDeveloperTools();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ConfigureLoggers(configuration);
            loggingBuilder.AddEventSourceLogger();

            loggingBuilder.AddOpenTelemetry(options =>
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

                var useOtlpExporter = string.IsNullOrWhiteSpace(configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]) is false;
                if (useOtlpExporter)
                {
                    options.AddOtlpExporter();
                }
            });

            loggingBuilder.AddEventLog(options => configuration.GetRequiredSection("Logging:EventLog").Bind(options));
        });

        services.AddOptions<ClientWindowsSettings>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
