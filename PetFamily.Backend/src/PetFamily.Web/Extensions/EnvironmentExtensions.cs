namespace PetFamily.Web.Extensions;

public static class EnvironmentExtensions
{
    public static bool IsDocker(this IWebHostEnvironment environment)
    {
        return environment.EnvironmentName == "Docker";
    }
}