using System.Text.Json.Serialization;
using PWAApi.ApiService.Attributes;

namespace PWAApi.ApiService.DataTransferObjects.PlantID
{
    public class PlantDTO
    {
        public string Type { get; set; } = string.Empty;

        public string ScientificName { get; set; } = string.Empty;

        public string CommonName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Family { get; set; } = string.Empty; 

        public string Genus { get; set; } = string.Empty;

        public string Species { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [AIDescription("Biome the plant is most likely to be found in")]
        public string Biome { get; set; } = string.Empty;

        public string Cycle { get; set; } = string.Empty;

        public string Layer { get; set; } = string.Empty;

        public string SoilType { get; set; } = string.Empty;

        [AIDescription("Preferred time of day for watering")]
        public WateringPeriod[] WateringPeriods { get; set;} = Array.Empty<WateringPeriod>();

        [AIDescription("The level of care required for the plant")]
        public RequirementLevel CareRequirement { get; set; } = RequirementLevel.VeryLow;

        public RequirementLevel LightRequirement { get; set; } = RequirementLevel.VeryLow;

        public RequirementLevel WaterRequirement { get; set; } = RequirementLevel.VeryLow;

        [AIDescription("Anatomical parts of the plant such as: petal, stem, leaf, bud, seed, etc.")]
        public List<AnatomicalPart> Anatomy { get; set; } = new List<AnatomicalPart>();

        public EdibilityDTO Edibility { get; set; } = new EdibilityDTO(true, string.Empty);

        public HardinessZoneDTO HardinessZone { get; set; }

        public DimensionDTO Height { get; set; }

        public IEnumerable<ImageDTO> Images { get; set; } = new List<ImageDTO>();

        [AIDescription("The daily maximum and minimum hours of light the plant requires on average for each month")]
        public LightDurationDTO[] LightDurations { get; set; } = new LightDurationDTO[12];

        public ToxicityDTO Toxicity { get; set; } = new ToxicityDTO(true, string.Empty, string.Empty);

        [AIDescription("The amount of water the plant requires on average for each month in milliliters")]
        public WaterConsumptionDTO[] WaterConsumptions { get; set; } = new WaterConsumptionDTO[12];

        public DimensionDTO Width { get; set; }
    }

    public class PlantMetadataDTO
    {
        public string ScientificName { get; set; } = string.Empty;
        public List<string> CommonNames { get; set; } = new List<string>();
        public int TaxonKey { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RequirementLevel
    {
        VeryLow = 0,
        Low = 1,
        Medium = 2,
        High = 3,
        VeryHigh = 5
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Month 
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WateringPeriod
    {
        EarlyMorning = 0,
        Morning = 1,
        Midday = 2,
        Afternoon = 3,
        LateAfternoon = 4,
        Night = 5
    }

    public record AnatomicalPart(string Name, List<string> Colors);

    public struct DimensionDTO
    {
        public decimal Value { get; set; }
        public string Unit { get; set; }

        public DimensionDTO(decimal value, string unit)
        {
            Value = value;
            Unit = unit;
        }
    }

    public struct HardinessZoneDTO
    {
        public string Min { get; set; }
        public string Max { get; set; }
    }

    public record EdibilityDTO(bool Edible, string Parts);

    public record ImageDTO(string? URL, string? License, string? LicenseUrl ,string? Attribution);

    public record LightDurationDTO(double Max, double Min, Month month);

    public record SeedDTO(string Description, string Germination, float GerminationTemperature, string GerminationTime, double Weight, string Color, string Shape, string Texture, string Type);

    public record ToxicityDTO(bool Toxic, string Organisms, string Parts);

    public record WaterConsumptionDTO(int Value, Month Month);
}
