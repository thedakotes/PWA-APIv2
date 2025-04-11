namespace PWAApi.ApiService.Attributes
{
    public class SkipAISchemaAttribute : System.Attribute
    {
        private bool skipAISchema = true;

        public SkipAISchemaAttribute(bool skipAISchema = true)
        {
            this.skipAISchema = skipAISchema;
        }
    }
}
