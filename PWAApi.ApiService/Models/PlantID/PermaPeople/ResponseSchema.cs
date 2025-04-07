namespace PWAApi.ApiService.Models.PlantID.PermaPeople
{
    public class ResponseSchema
    {
        public List<Plant>? Plants { get; set; }
    }
    public class Plant
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
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PlantDataItem> Data { get; set; } = new();
    }

    public class PlantDataItem
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
