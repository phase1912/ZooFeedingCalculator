using ZooFeedingCalculator.Models;

namespace ZooFeedingCalculator.Services;

internal interface IFeedingCalculator
{
    double CalculateTotalDailyCost(IReadOnlyCollection<Animal> animals, FoodPrices prices);
}