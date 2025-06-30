using ZooFeedingCalculator.Services;

namespace ZooFeedingCalculator.Tests.Services;

public class FileLoaderTests
{
    #region LoadPrices

    private readonly FileLoader _loader = new();

    [Fact]
    public void LoadPrices_ParsesCorrectValues()
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllLines(tempFile, [
            "Meat=21.5",
            "Fruit=13.75"
        ]);

        var prices = _loader.LoadPrices(tempFile);

        Assert.Equal(21.5m, prices.MeatPricePerKg);
        Assert.Equal(13.75m, prices.FruitPricePerKg);
    }

    [Fact]
    public void LoadPrices_IgnoresInvalidLines()
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllLines(tempFile, [
            "Something=999",
            "Meat=10",
            "Fruit=5"
        ]);

        var prices = _loader.LoadPrices(tempFile);

        Assert.Equal(10, prices.MeatPricePerKg);
        Assert.Equal(5, prices.FruitPricePerKg);
    }

    [Fact]
    public void LoadPrices_DefaultsToZero_WhenValuesMissing()
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllLines(tempFile, [
            "Invalid=foo"
        ]);

        var prices = _loader.LoadPrices(tempFile);

        Assert.Equal(0, prices.MeatPricePerKg);
        Assert.Equal(0, prices.FruitPricePerKg);
    }

    [Fact]
    public void LoadPrices_ThrowsException_WhenFileMissing()
    {
        Assert.Throws<FileNotFoundException>(() => _loader.LoadPrices("nonexistent.txt"));
    }

    [Fact]
    public void LoadPrices_HandlesCommaDecimalWithInvariantCulture()
    {
        string tempFile = Path.GetTempFileName();
        File.WriteAllLines(tempFile, [
            "Meat=21.5",
            "Fruit=10.3"
        ]);

        var prices = _loader.LoadPrices(tempFile);

        Assert.Equal(21.5m, prices.MeatPricePerKg);
        Assert.Equal(10.3m, prices.FruitPricePerKg);
    }

    #endregion

    #region LoadAnimals

    private string CreateTempCsv(string content)
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, content);
        return path;
    }

    private string CreateTempXml(string content)
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, content);
        return path;
    }

    [Fact]
    public void LoadAnimals_LoadsAnimalsWithMatchingSpecies()
    {
        var csv = CreateTempCsv("Lion;0.05;both;90%;\nZebra;0.04;fruit;");
        var xml = CreateTempXml(@"<Zoo>
<Lions>
  <Lion name='Simba' kg='160'/>
</Lions>
<Zebras>
  <Zebra name='Zelly' kg='120'/>
</Zebras>
</Zoo>");

        var animals = _loader.LoadAnimals(csv, xml);

        Assert.Collection(animals,
            a =>
            {
                Assert.Equal("Simba", a.Name);
                Assert.Equal("both", a.TypeOfFood);
                Assert.Equal("Lion", a.Specie);
                Assert.Equal(160, a.Weight);
                Assert.Equal(0.05, a.Rate);
                Assert.Equal(90, a.MeatPercentage);
            },
            a =>
            {
                Assert.Equal("Zelly", a.Name);
                Assert.Equal("fruit", a.TypeOfFood);
                Assert.Equal("Zebra", a.Specie);
                Assert.Equal(120, a.Weight);
                Assert.Equal(0.04, a.Rate);
                Assert.Null(a.MeatPercentage);
            }
        );
    }

    [Fact]
    public void LoadAnimals_SkipsUnknownSpecies()
    {
        var csv = CreateTempCsv("Lion;0.05;meat;90%");
        var xml = CreateTempXml("<Zoo><Giraffes><Giraffe name='Unknown' kg='200'/></Giraffes></Zoo>");

        var animals = _loader.LoadAnimals(csv, xml);

        Assert.Empty(animals);
    }

    [Fact]
    public void LoadAnimals_HandlesEmptyFilesGracefully()
    {
        var emptyCsv = CreateTempCsv("");
        var emptyXml = CreateTempXml("<Zoo></Zoo>");

        var animals = _loader.LoadAnimals(emptyCsv, emptyXml);

        Assert.Empty(animals);
    }

    [Fact]
    public void LoadAnimals_ParsesWeightWithInvariantCulture()
    {
        var csv = CreateTempCsv("Lion;0.05;meat;90%");
        var xml = CreateTempXml("<Zoo><Lions><Lion name='Simba' kg='160.5'/></Lions></Zoo>");

        var animals = _loader.LoadAnimals(csv, xml);

        Assert.Single(animals);
        Assert.Equal(160.5, animals.ElementAt(0).Weight);
    }

    [Fact]
    public void LoadAnimals_ParsesMultipleSpeciesFromGroups()
    {
        var csv = CreateTempCsv("Tiger;0.07;meat;80%\nWolf;0.06;meat;75%");
        var xml = CreateTempXml(@"<Zoo>
<Tigers>
  <Tiger name='Dante' kg='150'/>
</Tigers>
<Wolves>
  <Wolf name='Pon' kg='70'/>
</Wolves>
</Zoo>");

        var animals = _loader.LoadAnimals(csv, xml);

        Assert.Equal(2, animals.Count);
        Assert.Contains(animals, a => a is { Name: "Dante", TypeOfFood: "meat" });
        Assert.Contains(animals, a => a is { Name: "Pon", TypeOfFood: "meat" });
    }

    #endregion
}