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
            "meat" => (totalFoodKg, 0),
            "fruit" => (0, totalFoodKg),
            "both" => (
                totalFoodKg * (MeatPercentage ?? 0) / 100,
                totalFoodKg * (1 - (MeatPercentage ?? 0) / 100)
            ),
            _ => (0, 0)
        };
    }
}