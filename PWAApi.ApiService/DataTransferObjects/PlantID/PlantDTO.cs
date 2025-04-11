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
    }
}
