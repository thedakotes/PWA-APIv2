using System.Text.Json.Serialization;
using PWAApi.ApiService.Attributes;

namespace PWAApi.ApiService.DataTransferObjects.PlantID
{
    public class PlantIDDTO
    {
        public float Score { get; set; }
        public required PlantIDSpeciesDTO Species { get; set; }
        public required PlantIDImageDTO[] Images { get; set; }
        public string? GBIF_ID { get; set; }
        public string? POWO_ID { get; set; }
        public IUCNDTO? IUCN { get; set; }
    }

    public class PlantIDSpeciesDTO
    {
        public required string ScientificNameWithoutAuthor { get; set; }
        public required string ScientificNameAuthorship { get; set; }
        public required TaxonomicRankDTO Genus { get; set; }
        public required TaxonomicRankDTO Family { get; set; }
        public string[]? CommonNames { get; set; }
    }

    public class PlantIDImageDTO
    {
        public string? Organ { get; set; }
        public string? Author { get; set; }
        public string? License { get; set; }
        public DateTime Date { get; set; }
        public required string Url { get; set; }
        public string? Citation { get; set; }
    }

    public class TaxonomicRankDTO
    {
        public string? ScientificNameWithoutAuthor { get; set; }
        public string? ScientificNameAuthorship { get; set; }
        public required string ScientificName { get; set; }
    }

    public class IUCNDTO
    {
        public string? ID { get; set;}
        public string? Category { get; set; }
    }
}
