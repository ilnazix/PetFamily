using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Accounts.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Accounts_20250909180407 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "first_name",
            schema: "accounts",
            table: "admin_accounts",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "last_name",
            schema: "accounts",
            table: "admin_accounts",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "middle_name",
            schema: "accounts",
            table: "admin_accounts",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "first_name",
            schema: "accounts",
            table: "admin_accounts");

        migrationBuilder.DropColumn(
            name: "last_name",
            schema: "accounts",
            table: "admin_accounts");

        migrationBuilder.DropColumn(
            name: "middle_name",
            schema: "accounts",
            table: "admin_accounts");
    }
}
