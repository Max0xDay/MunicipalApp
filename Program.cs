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
        // Build configuration (appsettings + environment specific + environment variables)
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = configBuilder.Build();
        ConfigurationHolder.Configuration = configuration; // store for later access

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
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
