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

        public string Cycle { get; set; } = string.Empty;

        public string Layer { get; set; } = string.Empty;

        public string SoilType { get; set; } = string.Empty;

        public List<string> WateringPeriods { get; set;} = new List<string>();

        [AIDescription("The level of care required for the plant")]
        public RequirementLevel CareRequirement { get; set; } = RequirementLevel.VeryLow;

        public RequirementLevel LightRequirement { get; set; } = RequirementLevel.VeryLow;

        public RequirementLevel MaintenanceRequirement { get; set; } = RequirementLevel.VeryLow;

        public RequirementLevel WaterRequirement { get; set; } = RequirementLevel.VeryLow;

        public List<AnatomicalPart> Anatomy { get; set; } = new List<AnatomicalPart>();

        public EdibilityDTO Edibility { get; set; } = new EdibilityDTO(true, string.Empty);

        public HardinessZoneDTO HardinessZone { get; set; }

        public DimensionDTO Height { get; set; }

        public IEnumerable<ImageDTO> Images { get; set; } = new List<ImageDTO>();

        public LightDurationDTO LightDuration { get; set; }

        public ToxicityDTO Toxicity { get; set; } = new ToxicityDTO(true, string.Empty, string.Empty);

        public WaterFrequencyDTO WaterFrequency { get; set; }

        public DimensionDTO Width { get; set; }
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

    public struct LightDurationDTO
    {
        public double Max { get; set; }
        public double Min { get; set; }
        public string Unit { get; set; }

        public LightDurationDTO(double min, double max, string unit)
        {
            Max = max;
            Min = min;
            Unit = unit;
        }
    }

    public record ToxicityDTO(bool Toxic, string Organisms, string Parts);

    public struct WaterFrequencyDTO
    {
        public int Value { get; set; }
        public string Unit { get; set; }

        public WaterFrequencyDTO(int value, string unit) 
        {
            Value = value;
            Unit = unit;
        }
    }
}
