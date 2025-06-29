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
        var prices = new FoodPrices(20.0, 10.0);

        var result = _calculator.CalculateTotalDailyCost(animals, prices);

        Assert.Equal(100 * 0.05 * 20.0, result);
    }

    [Fact]
    public void CalculatesCorrectCost_ForHerbivore()
    {
        var animals = new List<Animal>
        {
            new("Bambi", "Cow", "fruit", 80, 0.04)
        };
        var prices = new FoodPrices(30.0, 15.0);

        var result = _calculator.CalculateTotalDailyCost(animals, prices);

        Assert.Equal(80 * 0.04 * 15.0, result);
    }

    [Fact]
    public void CalculatesCorrectCost_ForOmnivore_WithMeatPercentage()
    {
        var animals = new List<Animal>
        {
            new("Bear", "Beer", "both", 200, 0.06, 60.0)
        };
        var prices = new FoodPrices(25.0, 12.0);

        var expected = 7.2 * 25.0 + 4.8 * 12.0;

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
        var prices = new FoodPrices(22.0, 8.0);

        var expected = 100 * 0.05 * 8.0;

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
        var prices = new FoodPrices(30.0, 30.0);

        var result = _calculator.CalculateTotalDailyCost(animals, prices);

        Assert.Equal(0, result);
    }
}