using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Database;

namespace PetFamily.Web.Extensions;

public static  class WebAppExtensions
{
    public static void ApplyMigrations(
        this WebApplication app)
    {
        using var scope =  app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var migrators = scope.ServiceProvider.GetServices<IDbMigrator>();

        foreach (var migrator in migrators)
        {
            migrator.Migrate();
        }
    }
}