using System.Globalization;
using System.Xml.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using ZooFeedingCalculator.Models;
using ZooFeedingCalculator.Models.CsvMappers;
using ZooFeedingCalculator.Models.CsvModels;

namespace ZooFeedingCalculator.Services;

internal class FileLoader : IFileLoader
{
    public FoodPrices LoadPrices(string filePath)
    {
        try
        {
            decimal meatPrice = 0, fruitPrice = 0;
            foreach (var line in File.ReadAllLines(filePath))
            {
                if (line.StartsWith("Meat"))
                    meatPrice = decimal.Parse(line.Split('=')[1], CultureInfo.InvariantCulture);
                else if (line.StartsWith("Fruit"))
                    fruitPrice = decimal.Parse(line.Split('=')[1], CultureInfo.InvariantCulture);
            }

            return new FoodPrices(meatPrice, fruitPrice);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public IReadOnlyCollection<Animal> LoadAnimals(string animalCsvPath, string zooXmlPath)
    {
        try
        {
            var speciesData = new List<AnimalCsvModel>();
            using (var reader = new StreamReader(animalCsvPath))
            using (var csv = new CsvReader(reader,
                       new CsvConfiguration(CultureInfo.InvariantCulture)
                       {
                           MissingFieldFound = null,
                           Delimiter = ";",
                           HasHeaderRecord = false
                       }))
            {
                csv.Context.RegisterClassMap<AnimalCsvMap>();
                speciesData = csv.GetRecords<AnimalCsvModel>().ToList();
            }

            var zoo = XDocument.Load(zooXmlPath);
            var animals = new List<Animal>();
            
            foreach (var group in zoo.Root?.Elements() ?? [])
            {
                foreach (var element in group.Elements())
                {
                    string name = element.Attribute("name")?.Value ?? string.Empty;
                    string specie = element.Name.LocalName;
                    double weight = double.Parse(element.Attribute("kg")?.Value ?? "0", CultureInfo.InvariantCulture);

                    var speciesInfo = speciesData.FirstOrDefault(s =>
                        s.Species.Equals(specie, StringComparison.InvariantCultureIgnoreCase));
                    if (speciesInfo != null && !string.IsNullOrEmpty(name))
                    {
                        animals.Add(new Animal(
                            name,
                            specie,
                            speciesInfo.TypeOfFood,
                            weight,
                            speciesInfo.Rate,
                            speciesInfo.MeatPercentageParsed
                        ));
                    }
                    else
                    {
                        Console.WriteLine(
                            $"Information about this type of animal({specie}) not present in animals.csv and {specie} by name {name} will be ignored.");
                    }
                }
            }

            return animals;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}