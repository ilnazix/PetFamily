using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ValueObjectList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "social_media_list",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "requisites_list",
                table: "pets");

            migrationBuilder.AlterColumn<string>(
                name: "requisites",
                table: "volunteers",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}",
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "social_medias",
                table: "volunteers",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<string>(
                name: "photos",
                table: "pets",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");

            migrationBuilder.AddColumn<string>(
                name: "requisites",
                table: "pets",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "social_medias",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "photos",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "requisites",
                table: "pets");

            migrationBuilder.AlterColumn<string>(
                name: "requisites",
                table: "volunteers",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "jsonb");

            migrationBuilder.AddColumn<string>(
                name: "social_media_list",
                table: "volunteers",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "requisites_list",
                table: "pets",
                type: "jsonb",
                nullable: true);
        }
    }
}
