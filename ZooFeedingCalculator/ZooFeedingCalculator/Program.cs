using ZooFeedingCalculator;
using ZooFeedingCalculator.Models.Configuration;
using ZooFeedingCalculator.Services;

internal static class Program
{
    static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<ZooPaths>(context.Configuration.GetSection("ZooPaths"));
                services.AddSingleton<IFileLoader, FileLoader>();
                services.AddSingleton<IFeedingCalculator, FeedingCalculator>();
                services.AddSingleton<Application>();
            })
            .Build();

        var app = host.Services.GetRequiredService<Application>();
        app.Run();
    }
}