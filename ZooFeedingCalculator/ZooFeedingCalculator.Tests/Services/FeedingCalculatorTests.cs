using ZooFeedingCalculator.Models;
using ZooFeedingCalculator.Services;

namespace ZooFeedingCalculator.Tests.Services;

public class FeedingCalculatorTests
{
    private readonly FeedingCalculator _calculator = new();

    [Fact]
    public void CalculatesCorrectCost_ForCarnivore()
    {
        var animals = new List<Animal>
        {
            new("Simba", "Lien", "meat", 100, 0.05)
        };
        var prices = new FoodPrices(20.0m, 10.0m);

        var result = _calculator.CalculateTotalDailyCost(animals, prices);

        Assert.Equal(100 * 0.05m * 20.0m, result);
    }

    [Fact]
    public void CalculatesCorrectCost_ForHerbivore()
    {
        var animals = new List<Animal>
        {
            new("Bambi", "Cow", "fruit", 80, 0.04)
        };
        var prices = new FoodPrices(30.0m, 15.0m);

        var result = _calculator.CalculateTotalDailyCost(animals, prices);

        Assert.Equal(80 * 0.04m * 15.0m, result);
    }

    [Fact]
    public void CalculatesCorrectCost_ForOmnivore_WithMeatPercentage()
    {
        var animals = new List<Animal>
        {
            new("Bear", "Beer", "both", 200, 0.06, 60.0)
        };
        var prices = new FoodPrices(25.0m, 12.0m);

        var expected = 7.2m * 25.0m + 4.8m * 12.0m;

        var result = _calculator.CalculateTotalDailyCost(animals, prices);

        Assert.Equal(expected, result, 2);
    }

    [Fact]
    public void CalculatesCorrectCost_ForOmnivore_WithNullMeatPercentage()
    {
        var animals = new List<Animal>
        {
            new("Pig", "Pig", "both", 100, 0.05)
        };
        var prices = new FoodPrices(22.0m, 8.0m);

        var expected = 100 * 0.05m * 8.0m;

        var result = _calculator.CalculateTotalDailyCost(animals, prices);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReturnsZero_WhenNoAnimals()
    {
        var result = _calculator.CalculateTotalDailyCost(new List<Animal>(), new FoodPrices(10, 10));
        Assert.Equal(0, result);
    }

    [Fact]
    public void IgnoresUnknownAnimalTypes()
    {
        var animals = new List<Animal>
        {
            new("Unknown", "Unknown", "alien", 100, 0.05)
        };
        var prices = new FoodPrices(30.0m, 30.0m);

        var result = _calculator.CalculateTotalDailyCost(animals, prices);

        Assert.Equal(0, result);
    }
}