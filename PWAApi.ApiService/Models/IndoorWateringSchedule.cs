namespace PWAApi.ApiService.Models
{
    public class IndoorWateringSchedule
    {
        public string PlantName { get; set; } = string.Empty;

        public int RoomTemperature { get; set; }

        public int HumidityLevel { get; set; }

        public string LightExposure { get; set; } = string.Empty;

        public string Airflow { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string WateringMethod { get; set; } = string.Empty;

        public bool PotDrainage { get; set; }

        public string PotMaterial { get; set; } = string.Empty;

        public string SoilType { get; set; } = string.Empty;

        public DateTime LastWateredDate { get; set; }
    }
}
