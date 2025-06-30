namespace ZooFeedingCalculator.Models;

internal class FoodPrices(decimal meatPrice, decimal fruitPrice)
{
    public decimal MeatPricePerKg { get; } = meatPrice;
    public decimal FruitPricePerKg { get; } = fruitPrice;
}