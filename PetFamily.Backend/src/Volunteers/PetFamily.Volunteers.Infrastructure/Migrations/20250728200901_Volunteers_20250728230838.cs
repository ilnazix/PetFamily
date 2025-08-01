using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Volunteers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Volunteers_20250728230838 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "volunteers");

            migrationBuilder.CreateTable(
                name: "volunteers",
                schema: "volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    work_experience_in_years = table.Column<int>(type: "integer", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    requisites = table.Column<string>(type: "text", nullable: false),
                    social_medias = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                schema: "volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_castrated = table.Column<bool>(type: "boolean", nullable: true),
                    is_vaccinated = table.Column<bool>(type: "boolean", nullable: true),
                    health_information = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    height = table.Column<int>(type: "integer", nullable: true),
                    weight = table.Column<int>(type: "integer", nullable: true),
                    country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    state = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    street = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    house_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    apartment_number = table.Column<int>(type: "integer", nullable: true),
                    owner_phone_number = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    requisites = table.Column<string>(type: "text", nullable: false),
                    photos = table.Column<string>(type: "text", nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    breed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pets", x => x.id);
                    table.ForeignKey(
                        name: "fk_pets_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalSchema: "volunteers",
                        principalTable: "volunteers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_pets_volunteer_id",
                schema: "volunteers",
                table: "pets",
                column: "volunteer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pets",
                schema: "volunteers");

            migrationBuilder.DropTable(
                name: "volunteers",
                schema: "volunteers");
        }
    }
}
