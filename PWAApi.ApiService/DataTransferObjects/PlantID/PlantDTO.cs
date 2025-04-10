namespace PWAApi.ApiService.DataTransferObjects.PlantID
{
    public class PlantDTO
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string ScientificName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Version { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public string Slug { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public EdibilityDTO Edibility { get; set; } = new EdibilityDTO(false, []);
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
        public List<string> Parts { get; set; } = new List<string>();

        public EdibilityDTO(bool edible, IEnumerable<string> parts)
        {
            Edible = edible;
            Parts = parts.ToList();
        }
    }
}
