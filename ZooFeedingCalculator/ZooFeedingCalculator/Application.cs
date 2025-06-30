using Microsoft.Extensions.Options;
using ZooFeedingCalculator.Models.Configuration;
using ZooFeedingCalculator.Services;

namespace ZooFeedingCalculator;

internal sealed class Application(
    IFileLoader fileLoader,
    IFeedingCalculator calculator,
    IOptions<ZooPaths> options)
{
    private readonly ZooPaths _paths = options.Value;

    public void Run()
    {
        var prices = fileLoader.LoadPrices(_paths.PricesPath);
        var animals = fileLoader.LoadAnimals(_paths.AnimalsCsvPath, _paths.ZooXmlPath);
        double totalCost = calculator.CalculateTotalDailyCost(animals, prices);

        Console.WriteLine($"Total daily feeding cost: {totalCost:F2} zł");
    }
}