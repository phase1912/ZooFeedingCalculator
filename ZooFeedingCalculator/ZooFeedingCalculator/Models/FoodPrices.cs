namespace ZooFeedingCalculator.Models;

internal class FoodPrices(double meatPrice, double fruitPrice)
{
    public double MeatPricePerKg { get; } = meatPrice;
    public double FruitPricePerKg { get; } = fruitPrice;
}