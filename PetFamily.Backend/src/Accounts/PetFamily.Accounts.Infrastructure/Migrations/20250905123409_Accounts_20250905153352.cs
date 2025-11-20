using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Accounts.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Accounts_20250905153352 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_role_permission_permission_permission_id",
            schema: "accounts",
            table: "role_permission");

        migrationBuilder.AddColumn<string>(
            name: "requisites",
            schema: "accounts",
            table: "volunteer_accounts",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "social_medias",
            schema: "accounts",
            table: "volunteer_accounts",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddForeignKey(
            name: "fk_role_permission_permissions_permission_id",
            schema: "accounts",
            table: "role_permission",
            column: "permission_id",
            principalSchema: "accounts",
            principalTable: "permissions",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "fk_role_permission_permissions_permission_id",
            schema: "accounts",
            table: "role_permission");

        migrationBuilder.DropColumn(
            name: "requisites",
            schema: "accounts",
            table: "volunteer_accounts");

        migrationBuilder.DropColumn(
            name: "social_medias",
            schema: "accounts",
            table: "volunteer_accounts");

        migrationBuilder.AddForeignKey(
            name: "fk_role_permission_permission_permission_id",
            schema: "accounts",
            table: "role_permission",
            column: "permission_id",
            principalSchema: "accounts",
            principalTable: "permissions",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade);
    }
}
