using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace PWAApi.ApiService.Models.Taxonomy
{
    public class Taxonomy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Name("taxonKey")]
        public required int TaxonKey { get; set; }

        [Name("scientificName")]
        public required string ScientificName { get; set; }

        [Name("acceptedTaxonKey")]
        public int AcceptedTaxonKey { get; set; }

        [Name("acceptedScientificName")]
        public required string AcceptedScientificName { get; set; }

        [Name("numberOfOccurrences")]
        public int NumberOfOccurences { get; set; }

        [Name("taxonRank")]
        public required string TaxonRank { get; set; }

        [Name("taxonomicStatus")]
        public required string TaxonomicStatus { get; set; }

        [Name("kingdom")]
        public required string Kingdom { get; set; }

        [Name("kingdomKey")]
        public int? KingdomKey { get; set; }

        [Name("phylum")]
        public required string Phylum { get; set; }

        [Name("phylumKey")]
        public int? PhylumKey { get; set; }

        [Name("class")]
        public required string Class { get; set; }

        [Name("classKey")]
        public int? ClassKey { get; set; }

        [Name("order")]
        public required string Order { get; set; }

        [Name("orderKey")]
        public int? OrderKey { get; set; }

        [Name("family")]
        public required string Family { get; set; }

        [Name("familyKey")]
        public int? FamilyKey { get; set; }

        [Name("genus")]
        public required string Genus { get; set; }

        [Name("genusKey")]
        public int? GenusKey { get; set; }

        [Name("species")]
        public required string Species { get; set; }

        [Name("speciesKey")]
        public int? SpeciesKey { get; set; }

        [Name("iucnRedListCategory")]
        public string? IUCNRedListCategory { get; set; }

        public virtual ICollection<VernacularName> VernacularNames { get; set; } = new HashSet<VernacularName>();
    }
}
