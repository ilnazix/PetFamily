using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.VolunteerRequest.Infrastructure.Migrations;

/// <inheritdoc />
public partial class VolunteerRequest_20251014160404 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "rejected_at",
            schema: "volunteer_requests",
            table: "volunteer_requests",
            type: "timestamp with time zone",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "rejected_at",
            schema: "volunteer_requests",
            table: "volunteer_requests");
    }
}
