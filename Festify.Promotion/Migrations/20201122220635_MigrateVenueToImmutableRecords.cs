using Microsoft.EntityFrameworkCore.Migrations;

namespace Festify.Promotion.Migrations
{
    public partial class MigrateVenueToImmutableRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO VenueDescription
	                (VenueId, Name, City, ModifiedDate)
                SELECT VenueId, Name, City, GETUTCDATE()
                FROM Venue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE v
                SET
	                Name = d.Name,
	                City = d.City
                FROM Venue v
                JOIN (
	                SELECT VenueId, Name, City,
		                ROW_NUMBER() OVER (
			                PARTITION BY VenueId
			                ORDER BY ModifiedDate DESC
		                ) AS row
	                FROM VenueDescription
                ) as d
                    ON v.VenueId = d.VenueId
                    AND d.row = 1
                ");
        }
    }
}
