using ZooFeedingCalculator.Models;

namespace ZooFeedingCalculator.Services;

internal interface IFeedingCalculator
{
    decimal CalculateTotalDailyCost(IReadOnlyCollection<Animal> animals, FoodPrices prices);
}