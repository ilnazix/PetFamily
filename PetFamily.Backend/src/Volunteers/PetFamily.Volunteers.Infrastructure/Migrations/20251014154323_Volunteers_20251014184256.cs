using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Volunteers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Volunteers_20251014184256 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "requisites",
                schema: "volunteers",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "social_medias",
                schema: "volunteers",
                table: "volunteers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "requisites",
                schema: "volunteers",
                table: "volunteers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "social_medias",
                schema: "volunteers",
                table: "volunteers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
