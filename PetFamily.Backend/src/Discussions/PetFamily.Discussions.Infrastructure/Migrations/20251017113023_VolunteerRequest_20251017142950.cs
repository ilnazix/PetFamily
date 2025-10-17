using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Discussions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VolunteerRequest_20251017142950 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "users",
                schema: "discussions",
                table: "discussions",
                newName: "participant_ids");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "participant_ids",
                schema: "discussions",
                table: "discussions",
                newName: "users");
        }
    }
}
