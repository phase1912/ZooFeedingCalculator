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
            })
            .Build();

        var config = host.Services.GetRequiredService<IConfiguration>();
        var fileLoader = host.Services.GetRequiredService<IFileLoader>();
        var calculator = host.Services.GetRequiredService<IFeedingCalculator>();

        var paths = config.GetSection("ZooPaths").Get<ZooPaths>();

        var prices = fileLoader.LoadPrices(paths.PricesPath);
        var animals = fileLoader.LoadAnimals(paths.AnimalsCsvPath, paths.ZooXmlPath);

        double totalCost = calculator.CalculateTotalDailyCost(animals, prices);
        Console.WriteLine($"Total daily feeding cost: {totalCost:F2} zł");
    }
}