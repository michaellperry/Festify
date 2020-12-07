using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Festify.Promotion.Migrations
{
    public partial class DefineVenueGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VenueGuid",
                table: "Venue",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Venue_VenueGuid",
                table: "Venue",
                column: "VenueGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Venue_VenueGuid",
                table: "Venue");

            migrationBuilder.DropColumn(
                name: "VenueGuid",
                table: "Venue");
        }
    }
}
