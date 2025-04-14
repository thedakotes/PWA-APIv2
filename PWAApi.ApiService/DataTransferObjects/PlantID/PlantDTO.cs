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
        public string Link { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public EdibilityDTO Edibility { get; set; } = new EdibilityDTO();
        public ToxicitityDTO Toxicity { get; set; } = new ToxicitityDTO();
        public ImageDTO Image { get; set; } = new ImageDTO();
        public string Growth { get; set; } = string.Empty;
        public string WaterRequirement { get; set; } = string.Empty;
        public string LightRequirement { get; set; } = string.Empty;
        public string USDAHardinessZone { get; set; } = string.Empty;
        public string Layer { get; set; } = string.Empty;
        public string SoilType { get; set; } = string.Empty;
    }

    public class EdibilityDTO
    {
        public bool Edible { get; set; }
        public string Parts { get; set; } = string.Empty;

        public EdibilityDTO() { }

        public EdibilityDTO(bool edible, string parts)
        {
            Edible = edible;
            Parts = parts;
        }
    }

    public class ImageDTO
    {
        public string URL { get; set; } = string.Empty;
        public string License { get; set; } = string.Empty;
        public string LicenseUrl { get; set; } = string.Empty;
        public string Attribution { get; set; } = string.Empty;

        public ImageDTO() { }

        public ImageDTO(string url, string license, string licenseUrl, string attribution)
        {
            URL = url;
            License = license;
            LicenseUrl = licenseUrl;
            Attribution = attribution;
        }
    }

    public class ToxicitityDTO
    {
        public bool Toxic { get; set; }
        public string Organisms { get; set; } = string.Empty;
        public string Parts { get; set; } = string.Empty;
    }
}
