using ZooFeedingCalculator.Models;

namespace ZooFeedingCalculator.Services;

internal class FeedingCalculator : IFeedingCalculator
{
    public double CalculateTotalDailyCost(IReadOnlyCollection<Animal> animals, FoodPrices prices)
    {
        double total = 0;
        foreach (var animal in animals)
        {
            var (meatKg, fruitKg) = animal.GetDailyFood();
            total += meatKg * prices.MeatPricePerKg + fruitKg * prices.FruitPricePerKg;
        }
        return total;
    }
}