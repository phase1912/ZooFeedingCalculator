using System.Globalization;

namespace ZooFeedingCalculator.Models.CsvModels;

internal sealed class AnimalCsvModel
{
    public string Species { get; set; }

    public string TypeOfFood { get; set; }

    public double Rate { get; set; }

    public string MeatPercentage { get; set; }

    public double? MeatPercentageParsed
    {
        get
        {
            if (string.IsNullOrWhiteSpace(MeatPercentage)) return null;
            var trimmed = MeatPercentage.Trim().Replace("%", "");
            return double.TryParse(trimmed, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
                ? result
                : null;
        }
    }
}