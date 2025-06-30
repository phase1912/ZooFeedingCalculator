using ZooFeedingCalculator.Models;

namespace ZooFeedingCalculator.Services;

internal class FeedingCalculator : IFeedingCalculator
{
    public decimal CalculateTotalDailyCost(IReadOnlyCollection<Animal> animals, FoodPrices prices)
    {
        decimal total = 0.0m;
        foreach (var animal in animals)
        {
            var (meatKg, fruitKg) = animal.GetDailyFood();
            total += (decimal)meatKg * prices.MeatPricePerKg + (decimal)fruitKg * prices.FruitPricePerKg;
        }
        return total;
    }
}