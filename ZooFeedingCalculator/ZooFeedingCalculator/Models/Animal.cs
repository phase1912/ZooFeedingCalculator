namespace ZooFeedingCalculator.Models;

internal class Animal(string name, string specie, string typeOfFood, double weight, double rate, double? meatPercentage = null)
{
    public string Name { get; } = name;
    
    public string TypeOfFood { get; } = typeOfFood.ToLower();
    
    public double Weight { get; } = weight;
    
    public double Rate { get; } = rate;
    
    public double? MeatPercentage { get; } = meatPercentage;

    public string Specie { get; set; } = specie;

    public (double meatKg, double fruitKg) GetDailyFood()
    {
        double totalFoodKg = Weight * Rate;
        return TypeOfFood switch
        {
            "meat" => (totalFoodKg, 0.0),
            "fruit" => (0.0, totalFoodKg),
            "both" => (
                totalFoodKg * (MeatPercentage ?? 0.0) / 100.0,
                totalFoodKg * (1.0 - (MeatPercentage ?? 0.0) / 100.0)
            ),
            _ => (0.0, 0.0)
        };
    }
}