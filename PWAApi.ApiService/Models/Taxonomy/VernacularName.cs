using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;

namespace PWAApi.ApiService.Models.Taxonomy
{
    public class VernacularName
    {
        [Ignore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Name("id")]
        public int? TaxonKey { get; set; }

        [Name("vernacularName")]
        public required string Name { get; set; } 

        [Name("source")]
        public string? Source { get; set; }

        [Name("language")]
        public string? Language { get; set; }

        [Name("countryCode")]
        public string? CountryCode { get; set; }

        [Name("sex")]
        public string? Sex { get; set; }

        [Name("lifeStage")]
        public string? LifeStage { get; set; }

        [Name("isPreferredName")]
        public string? IsPreferredName { get; set; }

        [Ignore]
        [ForeignKey(nameof(TaxonKey))]
        public virtual Taxonomy? Taxonomy { get; set; }
    }
}
