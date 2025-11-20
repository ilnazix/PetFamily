namespace PetFamily.VolunteerRequest.Infrastructure;
internal static class Constants
{
    public const string SCHEMA = "volunteer_requests";
    public const string WRITE_DB_CONTEXT_CONFIGURATIONS = "Configurations.Write";
    public const string READ_DB_CONTEXT_CONFIGURATIONS = "Configurations.Read";
    public const string DB_CONFIGURATION_SECTION = "Database";

    public static class Resilience 
    {
        public const string BasicPipeline = "basic-resilience-pipeline";
    }
}
