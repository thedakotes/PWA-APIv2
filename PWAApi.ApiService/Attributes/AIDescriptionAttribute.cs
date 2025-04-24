namespace PWAApi.ApiService.Attributes
{
    public class AIDescriptionAttribute : Attribute
    {
        public readonly string Description = string.Empty;

        public AIDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
