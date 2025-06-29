using CsvHelper.Configuration;
using ZooFeedingCalculator.Models.CsvModels;

namespace ZooFeedingCalculator.Models.CsvMappers;

internal sealed class AnimalCsvMap : ClassMap<AnimalCsvModel>
{
    public AnimalCsvMap()
    {
        Map(m => m.Species).Index(0);
        Map(m => m.Rate).Index(1);
        Map(m => m.TypeOfFood).Index(2);
        Map(m => m.MeatPercentage).Index(3).Optional();
    }
}