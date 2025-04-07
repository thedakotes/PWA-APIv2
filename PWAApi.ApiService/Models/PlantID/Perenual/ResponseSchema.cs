namespace PWAApi.ApiService.Models.PlantID.Perenual
{
    public class ResponseSchema
    {
        public List<Plant> Data { get; set; } = new();
        public int To { get; set; }
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }
        public int From { get; set; }
        public int LastPage { get; set; }
        public int Total { get; set; }
    }

    public class Plant
    {
        public int Id { get; set; }
        public string? CommonName { get; set; }
        public List<string>? ScientificName { get; set; }
        public List<string>? OtherName { get; set; }
        public string? Family { get; set; }
        public string? Hybrid { get; set; }
        public string? Authority { get; set; }
        public string? Subspecies { get; set; }
        public string? Cultivar { get; set; }
        public string? Variety { get; set; }
        public string? SpeciesEpithet { get; set; }
        public string? Genus { get; set; }
        public PlantImage? DefaultImage { get; set; }
    }

    public class PlantImage
    {
        public int ImageId { get; set; }
        public int License { get; set; }
        public string? LicenseName { get; set; }
        public string? LicenseUrl { get; set; }
        public string? OriginalUrl { get; set; }
        public string? RegularUrl { get; set; }
        public string? MediumUrl { get; set; }
        public string? SmallUrl { get; set; }
        public string? Thumbnail { get; set; }
    }

}
