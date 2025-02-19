using Microsoft.EntityFrameworkCore;
using PetFamily.Infrastructure.DbContexts;

namespace PetFamily.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationWriteDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
