using Microsoft.EntityFrameworkCore.Migrations;

namespace Festify.Promotion.Migrations
{
    public partial class AddAlternateKeysToVenueTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VenueTimeZone_VenueId",
                table: "VenueTimeZone");

            migrationBuilder.DropIndex(
                name: "IX_VenueLocation_VenueId",
                table: "VenueLocation");

            migrationBuilder.DropIndex(
                name: "IX_VenueDescription_VenueId",
                table: "VenueDescription");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_VenueTimeZone_VenueId_ModifiedDate",
                table: "VenueTimeZone",
                columns: new[] { "VenueId", "ModifiedDate" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_VenueLocation_VenueId_ModifiedDate",
                table: "VenueLocation",
                columns: new[] { "VenueId", "ModifiedDate" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_VenueDescription_VenueId_ModifiedDate",
                table: "VenueDescription",
                columns: new[] { "VenueId", "ModifiedDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_VenueTimeZone_VenueId_ModifiedDate",
                table: "VenueTimeZone");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_VenueLocation_VenueId_ModifiedDate",
                table: "VenueLocation");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_VenueDescription_VenueId_ModifiedDate",
                table: "VenueDescription");

            migrationBuilder.CreateIndex(
                name: "IX_VenueTimeZone_VenueId",
                table: "VenueTimeZone",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueLocation_VenueId",
                table: "VenueLocation",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueDescription_VenueId",
                table: "VenueDescription",
                column: "VenueId");
        }
    }
}
