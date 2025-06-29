using ZooFeedingCalculator.Models;

namespace ZooFeedingCalculator.Services;

internal interface IFileLoader
{
    FoodPrices LoadPrices(string filePath);
    IReadOnlyCollection<Animal> LoadAnimals(string animalCsvPath, string zooXmlPath);
}