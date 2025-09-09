using Avalonia;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MunicipalApp;

internal class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            // Build configuration (appsettings + environment specific + environment variables)
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
            var basePath = AppContext.BaseDirectory; // points to output folder with copied config files
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // make optional to avoid crash
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            ConfigurationHolder.Configuration = configBuilder.Build();
        }
        catch (Exception ex)
        {
            // Fallback: continue without configuration
            Console.WriteLine($"[Startup] Configuration load failed: {ex.Message}");
        }

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}

internal static class ConfigurationHolder
{
    public static IConfiguration? Configuration { get; set; }
}

public class MapboxOptions
{
    public string AccessToken { get; set; } = string.Empty;
    public string CountryFilter { get; set; } = "ZA";
    public int ResultLimit { get; set; } = 5;
    public string Types { get; set; } = "place,locality,neighborhood,address,poi";
}
